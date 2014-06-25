#region Copyright (c) 2014 Leonid Lezner. All rights reserved.
// Copyright (C) 2013-2014 Leonid Lezner
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using TCPlayer.API;
using TCPlayer.Interfaces;
using System.Windows.Forms;
using System.Threading;
using TCPlayer.API.Target;


namespace TCPlayer.Project
{
    public class TCProject : IConfigurable, ISerializable
    {
        /// <summary>
        /// Project version. If not equal to the version in the project file
        /// convert the content to the newer version
        /// </summary>
        private const string _PROJECT_VERSION = "0.1";
        
        /// <summary>
        /// Folder name for components
        /// </summary>
        private const string _COMPONENTS_DIR = "Components";
        
        /// <summary>
        /// Folder name for views
        /// </summary>
        private const string _VIEWS_DIR = "Views";

        /// <summary>
        /// This components should exist in every project
        /// </summary>
        private string[] _mandatoryComponents = { "Target" };

        /// <summary>
        /// Properties of the project's object
        /// </summary>
        private PropertySet _properties;

        /// <summary>
        /// All components in the project
        /// </summary>
        private DynComponentSet _components;
        
        /// <summary>
        /// All views in the project
        /// </summary>
        private DynViewSet _views;
        
        /// <summary>
        /// Project has some changes and can be saved
        /// </summary>
        private bool _hasChanges = false;
        
        /// <summary>
        /// Project is loaded
        /// </summary>
        private bool _loaded = false;
        
        /// <summary>
        /// Project is online (= connected to the target)
        /// </summary>
        private bool _isOnline = false;
        
        /// <summary>
        /// File path to the project
        /// </summary>
        private string _projectFilePath;

        public SynchronizationContext UIContext { get; set; }

        /// <summary>
        /// Public accessor for properties
        /// </summary>
        public PropertySet Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                _properties = value;
            }
        }

        /// <summary>
        /// Public accessor for components
        /// </summary>
        public DynComponentSet ComponentSet
        {
            get
            {
                return _components;
            }
            set
            {
                _components = value;
            }
        }

        /// <summary>
        /// Public accessor for views
        /// </summary>
        public DynViewSet ViewSet
        {
            get
            {
                return _views;
            }
            set
            {
                _views = value;
            }
        }

        public string ApplicationBasePath { get; set; }

        public string ProjectBasePath { get; set; }

        public delegate void GoToViewDelegate(string ViewName, string EntryPoint);
        
        public delegate void UINeedsUpdate(bool ViewsAndUI = false);

        public event UINeedsUpdate OnUINeedsUpdate;

        public event GoToViewDelegate GoToView;

        private ITarget _target;

        public Logger MainLogger { get; set; }

        public bool IsLoaded
        {
            get { return _loaded; }
            set { }
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                _hasChanges = value;
                if (_hasChanges && OnUINeedsUpdate != null)
                {
                    OnUINeedsUpdate();
                }
            }
        }

        public void RefreshViewsAndUI()
        {
            if (OnUINeedsUpdate != null)
            {
                OnUINeedsUpdate(true);
            }
        }

        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
            set
            {

            }
        }

        /// <summary>
        /// Returns the human readable address of the target
        /// </summary>
        public string TargetsFullAddress
        {
            get
            {
                try
                {
                    return _target.ReadableAddress;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private void CheckComponents()
        {
            foreach (string componentName in _mandatoryComponents)
            {
                if (ComponentSet[componentName] == null)
                {
                    throw new ProjectSerializationException(String.Format("Mandatory component '{0}' is missing", componentName));
                }
            }
        }

        public XElement Xml
        {
            get
            {
                XElement element = new XElement("Project");

                element.SetAttributeValue("Version", _PROJECT_VERSION);

                element.Add(Properties.Xml);
                element.Add(ComponentSet.Xml);
                element.Add(ViewSet.Xml);

                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "Project")
                {
                    throw new ProjectSerializationException("Creating Project with wrong XML element");
                }

                XAttribute VersionAttribute = Element.Attribute("Version");

                if (VersionAttribute == null)
                {
                    throw new ProjectSerializationException("Project file version is missing");
                }

                Element = AdaptProjectToCurrentVersion(Element, OldVersion: VersionAttribute.Value);

                // Loading project's properties
                Properties = PropertySet.FromParentXmlElement(Element);

                Properties.Changed += Properties_Changed;

                // Loading components
                ComponentSet = DynComponentSet.FromParentXmlElement(Element);

                ComponentSet.Parent = this;

                // Loading views
                ViewSet = DynViewSet.FromParentXmlElement(Element);

                ViewSet.Parent = this;
            }
        }

        void Properties_Changed(object sender, EventArgs e)
        {
            HasChanges = true;
        }

        private XElement AdaptProjectToCurrentVersion(XElement Element, string OldVersion)
        {
            return Element;
        }

        public string GetProjectInfo(string Name)
        {
            switch (Name)
            {
                case "ProjectFilePath":
                    return _projectFilePath;
                case "BasePath":
                    return ProjectBasePath;
                case "Title":
                    return Properties["Title"].Value;
            }

            return null;
        }

        public bool IsVerboseLoad()
        {
            return GetProperty<bool>("VerboseLoad", false);
        }

        public void Load(string FilePath, IProgressEx Progress = null)
        {
            _hasChanges = false;

            _projectFilePath = FilePath;

            ProjectBasePath = Path.GetDirectoryName(_projectFilePath);

            MainLogger.Log(String.Format("Loading Project File '{0}'...", Path.GetFileName(_projectFilePath)), 0,
                LogMessageType.Information, LogReceiver.Console | LogReceiver.StatusBar);

            XDocument xDoc = XDocument.Load(FilePath);

            // Deserializing the project from XML document
            this.Xml = xDoc.Element("Project");

            // Base directory where the components are stored
            ComponentSet.BaseDir = Path.Combine(ApplicationBasePath, _COMPONENTS_DIR);

            // Optional project directory where the components are stored
            ComponentSet.ProjectDir = Path.Combine(ProjectBasePath, _COMPONENTS_DIR);

            ComponentSet.VerboseLoad = IsVerboseLoad();

            // Loading components
            ComponentSet.Load(Progress);

            CheckComponents();

            // If views are defined, load them
            if (ViewSet != null)
            {
                ViewSet.VerboseLoad = ComponentSet.VerboseLoad;
                ViewSet.Components = ComponentSet;
                ViewSet.BaseDir = Path.Combine(ApplicationBasePath, _VIEWS_DIR);
                ViewSet.ProjectBaseDir = Path.Combine(ProjectBasePath, _VIEWS_DIR);
                ViewSet.Load(Progress);
            }

            if (ComponentSet.HasExceptions || ViewSet.HasExceptions)
            {
                MainLogger.Log(string.Format("Project '{0}' has loaded with problems!", Properties["Title"]), 0, 
                    LogMessageType.Exclamation, LogReceiver.Console | LogReceiver.StatusBar | LogReceiver.MessageBox);
            }
            else
            {
                MainLogger.Log(string.Format("Project '{0}' loaded successfully!", Properties["Title"]), 0, 
                    LogMessageType.Success, LogReceiver.Console | LogReceiver.StatusBar);
            }

            _target = (ITarget)ComponentSet["Target"].Plugin;

            _loaded = true;
        }

        public async Task LoadAsync(string FilePath, IProgressEx Progress = null)
        {
            await Task.Factory.StartNew(() => Load(FilePath, Progress));
        }

        public void Unload()
        {
            if (ComponentSet != null)
            {
                ComponentSet.Unload(null);
            }

            if (ViewSet != null)
            {
                ViewSet.Unload(null);
            }

            _loaded = false;
        }
        
        public async Task<bool> SaveAsync(IProgressEx Progress)
        {
            if ((await ComponentSet.SaveAsync(Progress)) == false)
            {
                return false;
            }

            if ((await ViewSet.SaveAsync(Progress)) == false)
            {
                return false;
            }

            try
            {
                XDocument xDoc = new XDocument();

                xDoc.Add(this.Xml);

                xDoc.Save(_projectFilePath);

                _hasChanges = false;
            }
            catch(Exception ex)
            {
                MainLogger.Log(ex);
                return false;
            }

            return true;
        }


        /// <summary>
        /// This function creates a tree of views for the tree control.
        /// </summary>
        /// <param name="ViewList"></param>
        /// <returns></returns>
        public List<TreeNode> GetTreeNodes(List<DynView> ViewList = null)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            if (ViewList == null)
            {
                ViewList = ViewSet.Views;
            }

            foreach (DynView view in ViewList)
            {
                if (!view.IsEnabled())
                {
                    continue;
                }

                TreeNode node = new TreeNode();

                node.Text = view.Title;

                if (view.Icon != null)
                {
                    node.ImageKey = view.Name;
                    node.SelectedImageKey = view.Name;
                }
                else
                {
                    node.ImageKey = "noIcon";
                    node.SelectedImageKey = node.ImageKey;
                }

                node.Name = view.Name;

                List<TreeNode> subNodes = GetTreeNodes(view.SubViews);

                if (subNodes.Count > 0)
                {
                    node.Nodes.AddRange(GetTreeNodes(view.SubViews).ToArray());
                }

                nodes.Add(node);
            }

            return nodes;
        }

        internal bool GoOnline(bool online = true, bool forceAction = false, IProgressEx Progress = null)
        {
            if (online)
            {
                try
                {
                    _target.Connect(Progress);
                }
                catch(Exception ex)
                {
                    MainLogger.Log(ex);
                    return false;
                }

                if (!forceAction)
                {
                    foreach (DynComponent component in ComponentSet)
                    {
                        if (!component.CanGoOnline())
                        {
                            MainLogger.Log(string.Format("Component '{0}' can not go online", component.Ident), 0, 
                                LogMessageType.Exclamation, LogReceiver.Console);
                            return false;
                        }
                    }

                    foreach (KeyValuePair<string, DynView> entry in ViewSet.ViewsList)
                    {
                        if (!entry.Value.CanGoOnline())
                        {
                            MainLogger.Log(string.Format("View '{0}' can not go online", entry.Value.Name), 0, 
                                LogMessageType.Exclamation, LogReceiver.Console);
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (!forceAction)
                {
                    foreach (DynComponent component in ComponentSet)
                    {
                        if (!component.CanGoOffline())
                        {
                            MainLogger.Log(string.Format("Component '{0}' can not go offline", component.Ident), 0, 
                                LogMessageType.Exclamation, LogReceiver.Console);
                            return false;
                        }
                    }

                    foreach (KeyValuePair<string, DynView> entry in ViewSet.ViewsList)
                    {
                        if (!entry.Value.CanGoOffline())
                        {
                            MainLogger.Log(string.Format("View '{0}' can not go offline", entry.Value.Name), 0, 
                                LogMessageType.Exclamation, LogReceiver.Console);
                            return false;
                        }
                    }
                }
            }

            foreach (DynComponent component in ComponentSet)
            {
                if (online)
                {
                    if (!component.GoOnline())
                    {
                        GoOffline(Progress);

                        if (!forceAction)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    component.GoOffline();
                }
            }
            
            foreach (KeyValuePair<string, DynView> entry in ViewSet.ViewsList)
            {
                if(online)
                {
                    if(!entry.Value.GoOnline())
                    {
                        GoOffline(Progress);

                        if (!forceAction)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    entry.Value.GoOffline();
                }
            }

            if (!online)
            {
                try
                {
                    _target.Disconnect(Progress);
                }
                catch (Exception)
                {

                }
            }

            _isOnline = online;

            return true;
        }

        public bool GoOffline(IProgressEx Progress, bool forceAction = false)
        {
            return GoOnline(false, forceAction, Progress);
        }

        public void CallUpdateCycle()
        {
            foreach (DynComponent component in ComponentSet)
            {
                component.CallUpdateCycle();
            }

            foreach (KeyValuePair<string, DynView> entry in ViewSet.ViewsList)
            {
                entry.Value.CallUpdateCycle();
            }
        }

        public async Task<bool> GoOnlineAsync(bool forceAction = false, IProgressEx Progress = null)
        {
            return await Task.Factory.StartNew(() => GoOnline(forceAction: forceAction, Progress: Progress));
        }

        public async Task<bool> GoOfflineAsync(IProgressEx Progress, bool forceAction = false)
        {
            return await Task.Factory.StartNew(() => GoOffline(Progress, forceAction));
        }

        public void OnGoToView(string ViewName, string EntryPoint)
        {
            if (GoToView != null)
            {
                GoToView(ViewName, EntryPoint);
            }
        }

        public string GetProperty(string Name, string DefaultValue = "")
        {
            return Properties[Name, DefaultValue].Value;
        }

        public void SetProperty(string Name, string NewValue)
        {
            bool valueHasChanged = Properties[Name].Value != NewValue;

            Properties[Name] = new Property(Name, NewValue);
        }

        public ToolStripItem[] GetMenuStripItems(string ComponentName)
        {
            return ComponentSet[ComponentName].MenuItems;
        }

        public ToolStripItem[] GetToolStripItems(string ComponentName)
        {
            return ComponentSet[ComponentName].ToolStripItems;
        }

        public ToolStripItem[] GetTargetMenuStripItems()
        {
            return GetMenuStripItems("Target");
        }

        public ToolStripItem[] GetTargetToolStripItems()
        {
            return GetToolStripItems("Target");
        }

        public bool IsConnectedToTarget()
        {
            if (_target == null)
            {
                return false;
            }

            try
            {
                return _target.IsConnected();
            }
            catch(Exception)
            {
                return false;
            }
        }

        public TType GetProperty<TType>(string Name, TType DefaultValue)
        {
            return Properties[Name, DefaultValue.ToString()].GetValue<TType>();
        }
    }
}

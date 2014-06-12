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
using TCPlayer.API;
using TCPlayer.API.Views;
using System.Windows.Forms;
using TCPlayer.Interfaces;
using System.Drawing;
using System.Threading;
using TCPlayer.API.Components;

namespace TCPlayer.Project
{
    public class DynView : MarshalByRefObject, ISerializable, IViewHost, IDisposable
    {
        #region Interface implementation

        public bool Online { get { return _online; } }
        public bool Visible { get { return _visible; } }
        public string Title
        {
            get
            {
                return string.IsNullOrEmpty(_title) ? Name : _title;
            }
            set
            {
                _title = value;
            }
        }
        public Image Icon { get; set; }
        public string Ident { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public Version Version { get; set; }
        public IView Parent { get { return ParentView.Plugin; } }

        // Menu items will be added in the main window
        public ToolStripItem[] MenuItems { get; set; }

        // Event is triggered when the view tab gets closed
        public event EventHandler<ViewEventArgs> OnClose;

        // Event is triggered when the view tab gets deselected
        public event EventHandler<ViewEventArgs> OnHide;

        // Event is triggered when the view tab gets selected
        public event EventHandler<ViewEventArgs> OnShow;

        // Event is triggered when the application connects to the target
        public event EventHandler<PluginEventArgs> OnGoOffline;

        // Event is triggered when the application disconnects from the target
        public event EventHandler<PluginEventArgs> OnGoOnline;

        // Event is triggered when the application loads the project
        public event EventHandler<PluginEventArgs> OnLoadProject;

        // Event is triggered when the application saves the project
        public event EventHandler<PluginEventArgs> OnSaveProject;

        // Event is triggered when the application refreshes the views
        public event EventHandler<ViewEventArgs> OnRefresh;

        // Event is triggered before the view appears
        public event EventHandler<ViewEventArgs> OnBeforeShow;

        public event EventHandler<ViewEventArgs> OnViewlessCall;

        public event EventHandler<PluginEventArgs> OnBeforeGoOffline;

        public event EventHandler<PluginEventArgs> OnBeforeGoOnline;

        public event EventHandler<PluginCustomEventArgs> OnCustomEvent;

        public event EventHandler<PluginEventArgs> OnUpdateCycle;

        #endregion

        internal PropertySet Properties { get; set; }
        internal string Name { get { return _name; } }
        internal string Target { get; set; }
        internal bool AutoOpen { get; set; }
        internal string EntryPoint { get; set; }
        internal DynViewSet ParentSet { get; set; }
        internal DynView ParentView { get; set; }
        internal IView Plugin { get; set; }
        internal UserControl ViewControl { get; set; }

        private bool _updateCycleDone = true;

        private List<IViewTimer> _viewTimers = new List<IViewTimer>();

        private List<DynView> _subViews = new List<DynView>();

        private bool _pluginLoaded = false;

        private bool _isDisposed = false;
        
        private string _name = null;
        
        private bool _online = false;
        
        private bool _visible = false;
        
        private IProgressEx _progress;
        
        private string _title;
        
        public List<DynView> SubViews
        {
            get { return _subViews; }
        }

        public XElement Xml
        {
            get
            {
                XElement element = new XElement("View");

                element.SetAttributeValue("Name", Name);

                element.SetAttributeValue("Ident", Ident);

                if (!String.IsNullOrEmpty(Target))
                {
                    element.SetAttributeValue("Target", Target);
                }

                if (!String.IsNullOrEmpty(EntryPoint))
                {
                    element.SetAttributeValue("EntryPoint", EntryPoint);
                }

                element.SetAttributeValue("AutoOpen", AutoOpen.ToString());

                element.Add(Properties.Xml);

                foreach (DynView subView in _subViews)
                {
                    element.Add(subView.Xml);
                }

                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "View")
                {
                    throw new ProjectSerializationException("Creating View with wrong XML element");
                }

                this.Properties = PropertySet.FromParentXmlElement(Element);

                this.Properties.Changed += Properties_Changed;

                // Getting the Ident attribute
                XAttribute AttributeIdent = Element.Attribute("Ident");

                if (AttributeIdent == null || String.IsNullOrEmpty(AttributeIdent.Value))
                {
                    throw new ProjectSerializationException("Creating View with no Ident attribute");
                }

                Ident = AttributeIdent.Value;

                // Getting the Name attribute
                _name = Utils.GetXStringAttribute(Element, "Name", Ident);

                // Getting the Target attribute
                Target = Utils.GetXStringAttribute(Element, "Target", "Main");

                // Getting the Entry Point attribute
                EntryPoint = Utils.GetXStringAttribute(Element, "EntryPoint", "");

                // AutoOpen attribute
                AutoOpen = Utils.GetXBoolAttribute(Element, "AutoOpen");

                if(!AutoOpen)
                {
                    AutoOpen = Utils.GetXBoolAttribute(Element, "AutoStart");
                }
                
                // Getting the views
                var subElements = Element.Elements("View");

                foreach (XElement subElement in subElements)
                {
                    DynView subView = new DynView(subElement, ParentView, ParentSet);
                    _subViews.Add(subView);
                }
            }
        }

        private void Properties_Changed(object sender, EventArgs e)
        {
            NotifyChanges();
        }

        public DynView()
        {
            OnClose = null;
        }

        public DynView(XElement Xml, DynView ParentView, DynViewSet ParentSet)
        {
            Author = "";
            Description = "";
            Version = new Version("0.0.0.0");
            Title = "";

            this.ParentSet = ParentSet;
            this.ParentView = this;
            this.Xml = Xml;
        }


        internal void Load(IProgressEx Progress)
        {
            _progress = Progress;

            try
            {
                if (Plugin.Ident != Ident)
                {
                    throw new ViewNotFoundException(String.Format("View '{0}' has a wrong Ident", Ident));
                }
            }
            catch (NotImplementedException)
            {
                throw new ViewNotFoundException(String.Format("View '{0}' does not implement the Ident property", Ident));
            }

            try
            {
                Plugin.PluginHost = this;
            }
            catch (NotImplementedException) 
            {
                throw new ViewNotFoundException(String.Format("View '{0}' does not implement the PluginHost property", Ident));                
            }

            try
            {
                Plugin.LoadPlugin(Progress);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, LogReceiver.Console);
                ParentSet.HasExceptions = true;
                _pluginLoaded = false;
                ViewControl = null;
                return;
            }

            try
            {
                if (Plugin is UserControl)
                {
                    ViewControl = (UserControl)Plugin;
                }
                else
                {
                    ViewControl = null;
                }
            }
            catch (Exception ex)
            {
                ViewControl = null;
                Logger.Log(ex, LogReceiver.Console);
                ParentSet.HasExceptions = true;
            }

            _pluginLoaded = true;
        }

        public void Unload(IProgressEx Progress)
        {
            if (_pluginLoaded == true)
            {
                Plugin.UnloadPlugin(Progress);

                _pluginLoaded = false;
                
                foreach(DynViewTimer timer in _viewTimers)
                {
                    timer.Dispose();
                }
            }

            _isDisposed = true;
        }

        private bool CallHandler(object Handler, EventArgs PresetArgs = null)
        {
            try
            {
                if (Handler != null)
                {
                    if(Handler is EventHandler<PluginEventArgs>)
                    {
                        PluginEventArgs args = new PluginEventArgs();
                        ((EventHandler<PluginEventArgs>)Handler)(this, args);
                        return !args.Cancel;
                    }

                    if (Handler is EventHandler<ViewEventArgs>)
                    {
                        ViewEventArgs args;

                        if (PresetArgs == null)
                        {
                            args = new ViewEventArgs();
                        }
                        else
                        {
                            args = PresetArgs as ViewEventArgs;
                        }

                        ((EventHandler<ViewEventArgs>)Handler)(this, args);
                        return !args.Cancel;
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex, LogReceiver.Console);
                return false;
            }
        }

        public bool Show(string EntryPoint)
        {
            if (_visible)
            {
                return true;
            }

            _visible = true;

            ViewEventArgs args = new ViewEventArgs();

            args.EntryPoint = EntryPoint;

            _visible = CallHandler(OnShow, args);

            return _visible;
        }

        public bool Hide()
        {
            if(!_visible)
            {
                return true;
            }

            _visible = false;

            bool ret = CallHandler(OnHide);

            _visible = !ret;

            return ret;
        }

        public bool Close()
        {
            _visible = false;

            bool ret = CallHandler(OnClose);
            
            _visible = !ret;

            return ret;
        }

        public bool ViewlessCall(string EntryPoint)
        {
            ViewEventArgs args = new ViewEventArgs();
            args.EntryPoint = EntryPoint;
            _visible = CallHandler(OnViewlessCall, args);
            return _visible;
        }

        public bool Refresh()
        {
            return CallHandler(OnRefresh);
        }

        public bool LoadProjectData()
        {
            return CallHandler(OnLoadProject);
        }

        public bool IsEnabled()
        {
            if(!_pluginLoaded)
            {
                return false;
            }

            return CallHandler(OnBeforeShow);
        }

        public bool CanGoOnline()
        {
            _online = CallHandler(OnBeforeGoOnline);
            return _online;
        }

        public bool CanGoOffline()
        {
            bool ret = CallHandler(OnBeforeGoOffline);

            if (ret)
            {
                _online = false;
            }

            return ret;
        }

        public async Task<bool> ShowAsync(string EntryPoint)
        {
            return await Task<bool>.Factory.StartNew(() => Show(EntryPoint));
        }

        public async Task<bool> HideAsync()
        {
            return await Task.Factory.StartNew(() => Hide());
        }

        public async Task<bool> CloseAsync()
        {
            return await Task.Factory.StartNew(() => Close());
        }

        public async Task<bool> ViewlessCallAsync(string EntryPoint)
        {
            return await Task<bool>.Factory.StartNew(() => ViewlessCall(EntryPoint));
        }

        public async Task<bool> RefreshAsync()
        {
            return await Task.Factory.StartNew(() => Refresh());
        }

        public async Task<bool> IsEnabledAsync()
        {
            return await Task.Factory.StartNew(() => IsEnabled());
        }

        public bool GoOnline()
        {
            return CallHandler(OnGoOnline);
        }

        public bool GoOffline()
        {
            return CallHandler(OnGoOffline);
        }

        public TType GetProperty<TType>(string Name, TType DefaultValue)
        {
            return Properties[Name, DefaultValue.ToString()].GetValue<TType>();
        }

        public string GetProperty(string Name, string DefaultValue = "")
        {
            return Properties[Name, DefaultValue].Value;
        }

        public void SetProperty(string Name, string NewValue)
        {
            bool valueChanged = Properties[Name].Value != NewValue;

            Properties[Name] = new Property(Name, NewValue);
        }

        public string GetProjectInfo(string Name)
        {
            return ParentSet.Parent.GetProjectInfo(Name);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (!_isDisposed)
            {
                if (Disposing)
                {
                    Unload(_progress);
                }
            }
        }

        public bool Save(IProgressEx Progress)
        {
            return CallHandler(OnSaveProject);
        }

        ~DynView()
        {
            Dispose(false);
        }

        /// <summary>
        /// Opens a view specified by it's name
        /// </summary>
        /// <param name="Name">View's name as in the project XML file</param>
        public void GoToView(string ViewName, string EntryPoint = null)
        {
            ParentSet.Parent.OnGoToView(ViewName, EntryPoint);
        }

        /// <summary>
        /// Tells the project object that some data was changed and
        /// user can press the disk button to save the project.
        /// </summary>
        public void NotifyChanges()
        {
            ParentSet.Parent.HasChanges = true;
        }

        public ILogger Logger
        {
            get
            {
                return ParentSet.Parent.MainLogger;
            }
            set
            {
            }
        }

        /// <summary>
        /// Returns a component by using it's type as a name
        /// </summary>
        /// <typeparam name="TComponent">Interface name of a component</typeparam>
        /// <returns>Castet IComponent</returns>
        public TComponent GetComponent<TComponent>() where TComponent : class
        {
            string type = typeof(TComponent).Name;
            return (TComponent)GetComponent(type.Substring(1));
        }

        /// <summary>
        /// Returns a component by it's name
        /// </summary>
        /// <param name="ComponentType">Component's type</param>
        /// <returns></returns>
        public IComponent GetComponent(string ComponentType)
        {
            return DynComponent.GetComponent(ParentSet.Components, ComponentType);
        }

        public void RefreshViews()
        {
            ParentSet.Parent.RefreshViewsAndUI();
        }

        public SynchronizationContext UIContext
        {
            get
            {
                return ParentSet.Parent.UIContext;
            }
        }

        public bool CallCustomEvent(string EventName, object EventParameter, object EventCaller, out object EventResult)
        {
            if (OnCustomEvent != null)
            {
                PluginCustomEventArgs args = new PluginCustomEventArgs();

                args.EventName = EventName;
                args.EventParameter = EventParameter;
                args.EventCaller = EventCaller;

                OnCustomEvent(EventCaller, args);

                EventResult = args.EventResult;

                return !args.Cancel;
            }
            else
            {
                EventResult = null;
                return false;
            }
        }

        public bool CallCustomEvent(string EventName, object EventParameter, object EventCaller)
        {
            object temp;
            return CallCustomEvent(EventName, EventParameter, EventCaller, out temp);
        }

        internal void CallUpdateCycle()
        {
            if (!_updateCycleDone || !Visible)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                _updateCycleDone = false;
                CallHandler(OnUpdateCycle);
                _updateCycleDone = true;
            });
        }

        public IView GetView(string ViewName)
        {
            try
            {
                return ParentSet.Parent.ViewSet.ViewsList[ViewName].Plugin;
            }
            catch(Exception)
            {
                return null;
            }
        }



        public IViewTimer CreateTimer(bool AutoStart = false)
        {
            DynViewTimer timer = new DynViewTimer(this);

            _viewTimers.Add(timer);

            if(AutoStart)
            {
                timer.Start();
            }

            return timer;
        }
    }
}

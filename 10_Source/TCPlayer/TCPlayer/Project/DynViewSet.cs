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

using TCPlayer.API;
using TCPlayer.API.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TCPlayer.Interfaces;
using System.Diagnostics;

namespace TCPlayer.Project
{
    public class DynViewSet : IDisposable, ISerializable, IPluginSet
    {
        private bool _isDisposed = false;
        
        private List<DynView> _views = new List<DynView>();
        
        private Dictionary<string, DynView> _flattenViewDictionary = new Dictionary<string, DynView>();

        public string BaseDir { get; set; }

        public string ProjectBaseDir { get; set; }
        
        public TCProject Parent { get; set; }
        
        public DynComponentSet Components { get; set; }

        public bool HasExceptions { get; set; }
        
        public bool VerboseLoad { get; set; }

        public Dictionary<string, DynView> ViewsList
        {
            get { return _flattenViewDictionary; }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (DynView entry in _views)
            {
                yield return entry;
            }
        }

        public List<DynView> Views
        {
            get
            {
                return _views;
            }
        }

        public XElement Xml
        {
            get
            {
                XElement element = new XElement("Views");
                
                foreach (DynView component in this)
                {
                    element.Add(component.Xml);
                }

                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "Views")
                {
                    throw new ProjectSerializationException("Creating View Set with wrong XML element");
                }

                var viewElements = Element.Elements("View");

                if (viewElements != null && viewElements.Count() > 0)
                {
                    foreach (XElement viewElement in viewElements)
                    {
                        // Creating the View element for the root level
                        DynView view = new DynView(viewElement, null, this);

                        // Adding the elements
                        _views.Add(view);
                    }
                }
            }
        }

        public static DynViewSet FromParentXmlElement(XElement Parent)
        {
            var viewsElement = Parent.Element("Views");
            return new DynViewSet(viewsElement);
        }

        public DynViewSet()
        {

        }

        public DynViewSet(XElement Xml)
        {
            this.Xml = Xml;
        }

        public void Load(IProgressEx Progress)
        {
            AssemblyLoader asmLoader = new AssemblyLoader();
            ProgressExValue pV = new ProgressExValue();

            // Flatten the tree of views, so it can be iterated
            _flattenViewDictionary = FlattenViewTree(_views);

            foreach (KeyValuePair<string, DynView> entry in _flattenViewDictionary)
            {
                string viewAssembly;
                string className;
                string nameSpace;

                string logMessage = string.Format(Resources.Messages.LoadingViewFile, entry.Value.Name);

                if (Progress != null)
                {
                    pV.MainStatus = logMessage;
                    Progress.Report(pV);
                }

                if (VerboseLoad)
                {
                    Parent.MainLogger.Log(logMessage, 0, LogMessageType.Information, LogReceiver.Console);
                }

                // Search for the View's DLL file
                if (!DiscoverViewAssembly(entry.Value, out viewAssembly, out nameSpace, out className))
                {
                    Parent.MainLogger.Log(new ViewNotFoundException(String.Format(Resources.Messages.CanNotFindView, entry.Value.Ident)), 
                        LogReceiver.Console | LogReceiver.MessageBox);
                    
                    HasExceptions = true;

                    continue;
                }

                string ViewPluginType = String.Format("{0}.{1}", nameSpace, className);

                IView plugin = null;

                try
                {
                    plugin = asmLoader.Load<IView>(viewAssembly, ViewPluginType);
                }
                catch (Exception ex)
                {
                    throw new ViewException(ex.Message);
                }

                if (plugin == null)
                {
                    throw new ViewException(String.Format(Resources.Messages.CanNotLoadView, entry.Value.Ident));
                }

                entry.Value.Plugin = plugin;
            }

            int i = 0;

            // Initializing the views (calling the LoadPlugin method)
            foreach (KeyValuePair<string, DynView> entry in _flattenViewDictionary)
            {
                if(entry.Value.Plugin == null)
                {
                    continue;
                }

                string logMessage = string.Format(Resources.Messages.InitializingView, entry.Value.Name);

                if (Progress != null)
                {
                    pV.MainStatus = logMessage;
                    pV.Percentage = i * 100 / _flattenViewDictionary.Count;
                    Progress.Report(pV);
                }

                if (VerboseLoad)
                {
                    Parent.MainLogger.Log(logMessage, 0, LogMessageType.Information, LogReceiver.Console);
                }

                entry.Value.Load(Progress);

                i++;
            }

            Progress.Report(null);
        }

        private Dictionary<string, DynView> FlattenViewTree(List<DynView> ViewList)
        {
            Dictionary<string, DynView> level = new Dictionary<string, DynView>();

            foreach (DynView view in ViewList)
            {
                try
                {
                    level.Add(view.Name, view);
                }
                catch (ArgumentException)
                {
                    throw new ViewException(string.Format(Resources.Messages.ViewAlreadyExists, 
                        view.Name, view.Ident));
                }

                var ret = FlattenViewTree(view.SubViews);

                if (ret.Count() > 0)
                {
                    level = MergeViewDicts(level, ret);
                }
            }

            return level;
        }

        private Dictionary<string, DynView> MergeViewDicts(Dictionary<string, DynView> dictA, Dictionary<string, DynView> dictB)
        {
            Dictionary<string, DynView> newDict = new Dictionary<string, DynView>();

            foreach(KeyValuePair<string, DynView> entry in dictA)
            {
                try
                {
                    newDict.Add(entry.Key, entry.Value);
                }
                catch(ArgumentException)
                {
                    throw new ViewException(string.Format(Resources.Messages.ViewAlreadyExists,
                        entry.Value.Name, entry.Value.Ident));
                }
            }

            foreach (KeyValuePair<string, DynView> entry in dictB)
            {
                try
                {
                    newDict.Add(entry.Key, entry.Value);
                }
                catch (ArgumentException)
                {
                    throw new ViewException(string.Format(Resources.Messages.ViewAlreadyExists,
                        entry.Value.Name, entry.Value.Ident));
                }
            }

            return newDict;
        }

        public void Save(IProgressEx Progress = null)
        {
            foreach (KeyValuePair<string, DynView> entry in _flattenViewDictionary)
            {
                if (!entry.Value.Save(Progress))
                {
                    throw new ComponentException(string.Format(Resources.Messages.CanNotSaveView, entry.Value.Name));
                }
            }
        }

        public async Task<bool> SaveAsync(IProgressEx Progress)
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    Save(Progress);
                    return true;
                }
                catch (ComponentException ex)
                {
                    Parent.MainLogger.Log(ex);
                    return false;
                }
            });
        }

        public void Unload(IProgressEx Progress = null)
        {
            foreach (DynView view in this)
            {
                view.Unload(Progress);
            }

            _views.Clear();
            
            _isDisposed = true;
        }

        private bool DiscoverViewAssembly(DynView view, out string FilePath, out string Namespace, out string ClassName)
        {
            view.Ident.Split('.');

            FilePath = null;
            ClassName = null;
            Namespace = null;

            // Folders that could contain the assemblies
            var searchFolders = new string[] { ProjectBaseDir, BaseDir };

            // Parts of the ident string
            var parts = view.Ident.Split('.');

            if (parts.Count() < 1)
            {
                return false;
            }

            string className = parts.Last();

            var pathParts = parts.Take(parts.Count() - 1);

            string path = Path.Combine(pathParts.ToArray());

            List<string> filePaths = new List<string>();
            List<string> pathes = new List<string>();

            filePaths.Add(Path.Combine(path, view.Ident + ".dll"));

            if (parts.Count() > 1)
            {
                filePaths.Add(Path.Combine(Path.Combine(parts.Take(parts.Count() - 2).ToArray()), 
                    String.Join(".", pathParts.ToArray()) + ".dll"));

                filePaths.Add(String.Join(".", parts.Take(parts.Count() - 1).ToArray()) + ".dll");
            }

            foreach (string fileName in filePaths)
            {
                foreach (string folder in searchFolders)
                {
                    string filePath = Path.Combine(folder, fileName);

                    if (VerboseLoad)
                    {
                        Console.WriteLine("Searching for view file: {0}", filePath);
                    }

                    // Check if the file exists
                    if (File.Exists(filePath))
                    {
                        FilePath = filePath;
                        ClassName = className;
                        Namespace = Path.GetFileNameWithoutExtension(FilePath);
                        return true;
                    }
                }
            }

            return false;
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
                    Unload();
                }
            }
        }

        ~DynViewSet()
        {
            Dispose(false);
        }
    }
}

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using TCPlayer.API;
using TCPlayer.Interfaces;
using System.Security.Permissions;
using System.Runtime.ExceptionServices;
using TCPlayer.API.Target;
using TCPlayer.API.Components;

namespace TCPlayer.Project
{
    public class DynComponentSet : ISerializable, IDisposable, IPluginSet
    {
        private Dictionary<string, DynComponent> _components = new Dictionary<string, DynComponent>();
        private bool _isDisposed = false;

        public string BaseDir { get; set; }
        public string ProjectDir { get; set; }
        public TCProject Parent { get; set; }
        public DynComponentSet() { }
        public bool HasExceptions { get; set; }
        public bool VerboseLoad { get; set; }

        public XElement Xml
        {
            get
            {
                XElement element = new XElement("Components");
                foreach (DynComponent component in this)
                {
                    element.Add(component.Xml);
                }
                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "Components")
                {
                    throw new ProjectSerializationException("Creating Component Set with wrong XML element");
                }

                var ComponentElements = Element.Elements("Component");

                if (ComponentElements == null || ComponentElements.Count() < 1)
                {
                    throw new ProjectSerializationException("Creating Component Set with no Components");
                }

                foreach (XElement CompElement in ComponentElements)
                {
                    DynComponent component = new DynComponent(CompElement);

                    if (_components.ContainsKey(component.ComponentType))
                    {
                        throw new ProjectSerializationException(string.Format("Component Type '{0}' is already defined", 
                            component.ComponentType));
                    }

                    component.Parent = this;

                    _components.Add(component.ComponentType, component);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<String, DynComponent> entry in _components)
            {
                yield return entry.Value;
            }
        }

        public DynComponentSet(XElement Xml)
        {
            this.Xml = Xml;
        }

        public void Load(IProgressEx Progress)
        {
            AssemblyLoader asmLoader = new AssemblyLoader();

            // Loading the assemblies
            foreach (DynComponent component in this)
            {
                string logMessage = string.Format("Loading component '{0}'...", component.Ident);

                if(Progress != null)
                {
                    ProgressExValue pV = new ProgressExValue();
                    pV.MainStatus = logMessage;
                    Progress.Report(pV);
                }

                if (VerboseLoad)
                {
                    Parent.MainLogger.Log(logMessage, 0, LogMessageType.Information, LogReceiver.Console);
                }

                string ComponentAssembly = DiscoverComponentAssembly(component);

                if (string.IsNullOrEmpty(ComponentAssembly))
                {
                    throw new ComponentNotFoundException(String.Format("Can not find the component '{0}' of type '{1}'", 
                        component.Ident, component.ComponentType));
                }

                string ComponentType = String.Format("{0}.{1}", component.Ident, component.ComponentType);

                try
                {
                    switch (component.ComponentType)
                    {
                        case "Target":
                            component.Plugin = asmLoader.Load<ITarget>(ComponentAssembly, ComponentType);
                            break;
                        default:
                            component.Plugin = asmLoader.Load<IComponent>(ComponentAssembly, ComponentType);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new ComponentException(ex.Message);
                }

                if (component.Plugin == null)
                {
                    throw new ComponentException(String.Format("Can not load the component '{0}'", component.Ident));
                }
            }

            int i = 0;

            // Loading the components
            foreach (DynComponent component in this)
            {
                string logMessage = string.Format("Initializing component '{0}'...", component.Ident);

                if (Progress != null)
                {
                    ProgressExValue pV = new ProgressExValue();
                    pV.MainStatus = logMessage;
                    pV.Percentage = i * 100 / this._components.Count;
                    Progress.Report(pV);
                }

                if (VerboseLoad)
                {
                    Parent.MainLogger.Log(logMessage, 0, LogMessageType.Information, LogReceiver.Console);
                }

                component.Load(Progress);

                i++;
            }

            Progress.Report(null);
        }

        private string DiscoverComponentAssembly(DynComponent component)
        {
            string path;

            path = Path.Combine(ProjectDir, component.Ident, component.Ident + ".dll");

            if (VerboseLoad)
            {
                Console.WriteLine("Searching for component file: {0}", path);
            }

            if (File.Exists(path))
            {
                return path;
            }

            path = Path.Combine(BaseDir, component.Ident, component.Ident + ".dll");

            if (VerboseLoad)
            {
                Console.WriteLine("Searching for component file: {0}", path);
            }

            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        public void Save(IProgressEx Progress)
        {
            foreach (DynComponent component in this)
            {
                if (!component.Save(Progress))
                {
                    throw new ComponentException(string.Format("Can not save the component '{0}'", component.Ident));
                }
            }
        }

        public async Task<bool> SaveAsync(IProgressEx Progress)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    Save(Progress);
                    return true;
                }
                catch(ComponentException ex)
                {
                    Parent.MainLogger.Log(ex);
                    return false;
                }
            });
        }

        public DynComponent this[string Name]
        {
            get
            {
                try
                {
                    if (_components[Name].IsLoaded)
                    {
                        return _components[Name];
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }
            set
            {

            }
        }

        public void Unload(IProgressEx Progress)
        {
            foreach (DynComponent component in this)
            {
                if (component.Plugin != null)
                {
                    component.Unload(Progress);
                }
            }
        }

        public static DynComponentSet FromParentXmlElement(XElement Parent)
        {
            var ComponentsElement = Parent.Element("Components");
            return new DynComponentSet(ComponentsElement);
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
                    Unload(null);
                }
            }
        }

        ~DynComponentSet()
        {
            Dispose(false);
        }
    }
}

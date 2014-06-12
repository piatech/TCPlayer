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
using TCPlayer.API.Views;
using TCPlayer.API;
using System.Threading;
using TCPlayer.Interfaces;
using System.Windows.Forms;
using TCPlayer.API.Components;

namespace TCPlayer.Project
{
    public class DynComponent : MarshalByRefObject, IComponentHost, ISerializable, IDisposable
    {
        #region Interface implementation
        
        public string Author { get; set; }
        public string Description { get; set; }
        public Version Version { get; set; }
        public string Title { get; set; }

        // Menu items will be added in the main window
        public ToolStripItem[] MenuItems { get; set; }

        // Tool strip items will be added to the main toolbar
        public ToolStripItem[] ToolStripItems { get; set; }

        // Declaring Event Handlers
        public event EventHandler<PluginEventArgs> OnGoOffline;
        public event EventHandler<PluginEventArgs> OnGoOnline;
        public event EventHandler<PluginEventArgs> OnLoadProject;
        public event EventHandler<PluginEventArgs> OnSaveProject;
        public event EventHandler<PluginEventArgs> OnBeforeGoOffline;
        public event EventHandler<PluginEventArgs> OnBeforeGoOnline;
        public event EventHandler<PluginCustomEventArgs> OnCustomEvent;
        public event EventHandler<PluginEventArgs> OnUpdateCycle;

        #endregion

        internal PropertySet Properties
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
        internal string ComponentType { get; set; }
        internal string Ident { get; set; }
        internal DynComponentSet Parent { get; set; }
        internal IComponent Plugin { get; set; }
        internal bool IsLoaded
        {
            get
            {
                return _pluginLoaded;
            }
        }


        private PropertySet _properties;
        private bool _pluginLoaded = false;
        private bool _isDisposed = false;
        private IProgressEx _progress;
        private bool _updateCycleDone = true;


        public XElement Xml
        {
            get
            {
                XElement element = new XElement("Component");
                element.SetAttributeValue("Type", ComponentType);
                element.SetAttributeValue("Ident", Ident);
                element.Add(Properties.Xml);
                return element;
            }
            set
            {
                XElement Element = value;

                // Checking if the element name is "Component"
                if (Element == null || Element.Name != "Component")
                {
                    throw new ProjectSerializationException("Creating Component with wrong XML element");
                }

                this.Properties = PropertySet.FromParentXmlElement(Element);

                this.Properties.Changed += Properties_Changed;

                XAttribute AttributeType = Element.Attribute("Type");

                if (AttributeType == null || String.IsNullOrEmpty(AttributeType.Value))
                {
                    throw new ProjectSerializationException("Creating Component with no Type attribute");
                }

                ComponentType = AttributeType.Value;

                XAttribute AttributeIdent = Element.Attribute("Ident");

                if (AttributeIdent == null || String.IsNullOrEmpty(AttributeIdent.Value))
                {
                    throw new ProjectSerializationException("Creating Component with no Ident attribute");
                }

                Ident = AttributeIdent.Value;
            }
        }

        void Properties_Changed(object sender, EventArgs e)
        {
            NotifyChanges();
        }

        public DynComponent(XElement Xml)
        {
            this.Xml = Xml;
            Author = "";
            Description = "";
            Title = "";
            Version = new Version("0.0.0.0");
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

        public void Load(IProgressEx Progress)
        {
            try
            {
                if (Plugin.Ident != Ident)
                {
                    throw new ComponentNotFoundException(String.Format("Component '{0}' has a wrong Ident", ComponentType));
                }
            }
            catch(NotImplementedException)
            {
                throw new ComponentNotFoundException(String.Format("Component '{0}' has no Ident", ComponentType));
            }

            _progress = Progress;

            try
            {
                Plugin.PluginHost = this;
                Plugin.LoadPlugin(Progress);
            }
            catch(NotImplementedException)
            {
            }

            _pluginLoaded = true;
        }

        public bool Save(IProgressEx Progress)
        {
            return CallHandler(OnSaveProject);
        }

        public void Unload(IProgressEx Progress)
        {
            if (_pluginLoaded == true)
            {
                try
                {
                    Plugin.UnloadPlugin(Progress);
                }
                catch(NotImplementedException)
                {

                }

                _pluginLoaded = false;
            }
            _isDisposed = true;
        }

        private bool CallHandler(EventHandler<PluginEventArgs> Handler)
        {
            try
            {
                if (Handler != null)
                {
                    PluginEventArgs args = new PluginEventArgs();
                    Handler(this, args);
                    return !args.Cancel;
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

        public bool GoOnline()
        {
            return CallHandler(OnGoOnline);
        }

        public bool GoOffline()
        {
            return CallHandler(OnGoOffline);
        }

        public bool CanGoOnline()
        {
            return CallHandler(OnBeforeGoOnline);
        }

        public bool CanGoOffline()
        {
            return CallHandler(OnBeforeGoOffline);
        }

        public bool LoadProjectData()
        {
            return CallHandler(OnLoadProject);
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
                
        public static IComponent GetComponent(DynComponentSet Components, string ComponentType)
        {
            DynComponent comp = Components[ComponentType];

            if (comp != null)
            {
                return comp.Plugin;
            }
            else
            {
                return null;
            }
        }

        public IComponent GetComponent(string ComponentType)
        {
            return DynComponent.GetComponent(Parent, ComponentType);
        }

        public string GetProjectInfo(string Name)
        {
            return Parent.Parent.GetProjectInfo(Name);
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

        ~DynComponent() {
			Dispose(false);
		}

        /// <summary>
        /// Opens a view specified by it's name
        /// </summary>
        /// <param name="Name">View's name as in the project XML file</param>
        public void GoToView(string ViewName, string EntryPoint = null)
        {
            Parent.Parent.OnGoToView(ViewName, EntryPoint);
        }

        public void NotifyChanges()
        {
            Parent.Parent.HasChanges = true;
        }

        public void RefreshViews()
        {
            Parent.Parent.RefreshViewsAndUI();
        }


        public ILogger Logger
        {
            get
            {
                return Parent.Parent.MainLogger;
            }
            set
            {
            }
        }

        public TComponent GetComponent<TComponent>() where TComponent : class
        {
            var type = typeof(TComponent).Name;
            return (TComponent)GetComponent(type.Substring(1));
        }

        public SynchronizationContext UIContext
        {
            get
            {
                return Parent.Parent.UIContext;
            }
        }

        internal void CallUpdateCycle()
        {
            if(!_updateCycleDone)
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
                return Parent.Parent.ViewSet.ViewsList[ViewName].Plugin;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TType GetProperty<TType>(string Name, TType DefaultValue)
        {
            return Properties[Name, DefaultValue.ToString()].GetValue<TType>();
        }
    }
}
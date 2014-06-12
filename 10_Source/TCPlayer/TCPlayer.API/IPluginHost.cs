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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPlayer.API.Components;
using TCPlayer.API.Views;

namespace TCPlayer.API
{
    /// <summary>
    /// The Plugin Host is the public wrapper for the component object of
    /// TCPlayer. It provides small subset of function for the plugin.
    /// 
    /// The IConfigurable interface provides the methods for getting and
    /// setting properties of the component. Those properties are saved later
    /// in the project XML file.
    /// </summary>
    public interface IPluginHost : IConfigurable
    {
        string Author { get; set; }
        
        string Description { get; set; }

        Version Version { get; set; }

        string Title { get; set; }

        // Menu items will be added in the main window
        ToolStripItem[] MenuItems { get; set; }

        // The main logger object
        ILogger Logger { get; }

        // UI Context object for accessing the UI from different threads
        SynchronizationContext UIContext { get; }

        // Returns the generic component object by type
        IComponent GetComponent(string ComponentType);

        // Returns the component object of type TComponent
        TComponent GetComponent<TComponent>() where TComponent : class;

        IView GetView(string ViewName);

        // Notifies the the project, that some changes were made and user can save it
        void NotifyChanges();

        // Refreshing the main UI and opened views
        void RefreshViews();
        
        // Opening the view with provided name
        void GoToView(string ViewName, string EntryPoint = null);

        // This method can be called by user from any component or view
        bool CallCustomEvent(string EventName, object EventParameter, object EventCaller);
        bool CallCustomEvent(string EventName, object EventParameter, object EventCaller, out object EventResult);

        event EventHandler<PluginEventArgs> OnBeforeGoOnline;
        event EventHandler<PluginEventArgs> OnGoOnline;
        
        event EventHandler<PluginEventArgs> OnBeforeGoOffline;
        event EventHandler<PluginEventArgs> OnGoOffline;

        event EventHandler<PluginEventArgs> OnSaveProject;
        event EventHandler<PluginEventArgs> OnLoadProject;

        // Is called periodically
        event EventHandler<PluginEventArgs> OnUpdateCycle;

        // This event is fired when CallCustomEvent method gets called
        event EventHandler<PluginCustomEventArgs> OnCustomEvent;
    }
}
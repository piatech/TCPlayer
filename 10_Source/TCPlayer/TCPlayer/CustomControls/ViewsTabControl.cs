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
using System.Windows.Forms;
using TCPlayer.Project;
using TCPlayer.API;
using System.Drawing;
using System.Diagnostics;

namespace TCPlayer.CustomControls
{
    public class ViewsTabControl : TabControl
    {
        private int menuIndexOffset = 0;

        // Storing the current page that was right clicked
        private TabPage _hoveredPage = null;

        // Those tabs can not be closed
        public List<string> NotCloseableTabs { get; set; }

        TabPage _lastSelectedPage = null;

        private bool _forceDeselecting;
        private bool _isBusy;
        private TabPage _targetingPage;
        private string _lastEntryPoint;
        
        private bool IsBusy
        {
            get
            {
                return _isBusy || _isBusyForce;
            }
            set
            {
                _isBusy = value || _isBusyForce;

                if (OnLockUI != null && !_isBusyForce)
                {
                    OnLockUI(value);
                }
            }
        }

        private bool _isBusyForce = false;

        public TabPage LastSelectedTab { get { return _lastSelectedPage; } set { } }

        public DynViewSet ViewSet { get; set; }

        public Logger Logger { get; set; }

        public delegate void LockUI(bool locked);

        public event LockUI OnLockUI;

        public DynView CurrentView
        {
            get
            {
                try
                {
                    return ViewSet.ViewsList[SelectedTab.Name];
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        public ViewsTabControl()
        {
            // List of tabs without a context menu
            NotCloseableTabs = new List<string>();

            // Creating the context menu for tabs
            CreateContextMenu();

            // Callbacks for selecting and deselecting the tabs
            Selecting += ViewsTabControl_Selecting;
            Deselecting += ViewsTabControl_Deselecting;
        }

        private void CreateContextMenu()
        {
            // Creating a menu strip for the tab context menu
            ContextMenuStrip cms = new ContextMenuStrip();

            // Menu Item "Refresh"
            cms.Items.Add(Resources.Messages.Refresh, Resources.Resources.arrow_refresh, new EventHandler(OnMenuClick_Refresh));

            cms.Items.Add(new ToolStripSeparator());

            // Menu Item "Close"
            cms.Items.Add(Resources.Messages.Close, Resources.Resources.application_delete, new EventHandler(OnMenuClick_Close));
            
            // Menu Item "Close All"
            cms.Items.Add(Resources.Messages.CloseAll, null, new EventHandler(OnMenuClick_CloseAll));
            
            // Menu Item "Close All But This"
            cms.Items.Add(Resources.Messages.CloseAllButThis, null, new EventHandler(OnMenuClick_CloseAllButThis));

            // Offset for the menu with tab shortcuts
            menuIndexOffset = cms.Items.Count;

            ContextMenuStrip = cms;

            ContextMenuStrip.Opening += MenuOpeningEvent;
        }

        public async Task<bool> ShowViewAsync(TabPage Page)
        {
            var view = ViewSet.ViewsList[Page.Name];

            IsBusy = true;

            if ((await view.ShowAsync(_lastEntryPoint)) == true)
            {
                Page.Controls[0].Controls[0].Focus();

                // DELETE THIS: Logger.Log(string.Format("Showing {0}", view.Title), 0, LogMessageType.Information, LogReceiver.StatusBar);
                IsBusy = false;
                _lastEntryPoint = null;
                return true;
            }

            _lastEntryPoint = null;
            IsBusy = false;

            return false;
        }

        public async Task AddViewTab(string Name, string EntryPoint, bool ShowView = true)
        {
            var view = ViewSet.ViewsList[Name];
            _lastEntryPoint = EntryPoint;

            if ((await view.IsEnabledAsync()) == false)
            {
                return;
            }

            // Tab is already opened
            if (TabPages.ContainsKey(Name))
            {
                if(SelectedTab.Name == view.Name)
                {
                    await CallShowView(SelectedTab);
                    return;
                }

                // Select the tab
                _targetingPage = TabPages[view.Name];
                SelectTab(view.Name);
                return;
            }

            UserControl uc = view.ViewControl;

            if (uc == null)
            {
                await view.ViewlessCallAsync(view.EntryPoint);
                return;
            }

            // Wrapper for the view control
            Panel viewPanel = new Panel();

            // Set the size and the color
            viewPanel.BackColor = uc.BackColor;
            viewPanel.AutoScrollMinSize = uc.Size;
            viewPanel.AutoScroll = true;

            // Add the new control
            viewPanel.Controls.Add(uc);

            // Fill the whole area
            viewPanel.Dock = DockStyle.Fill;
            uc.Dock = DockStyle.Fill;

            // Adding a new tab
            TabPages.Add(view.Name, view.Title);

            // Displaying the new tab
            TabPages[view.Name].Show();

            TabPages[view.Name].BackColor = viewPanel.BackColor;

            if (view.Icon != null)
            {
                TabPages[view.Name].ImageKey = view.Name;
            }
            else
            {
                TabPages[view.Name].ImageKey = "noIcon";
            }

            TabPages[view.Name].Controls.Add(viewPanel);

            if (ShowView)
            {
                if (TabPages.Count > 1)
                {
                    _targetingPage = TabPages[view.Name];
                    SelectTab(view.Name);
                }
                else
                {
                    if(await ShowViewAsync(TabPages[view.Name]) == false)
                    {
                        TabPages.Remove(TabPages[view.Name]);
                    }
                }
            }
        }

        private async void ViewsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(DesignMode)
            {
                return;
            }

            if (e.TabPage == null)
            {
                return;
            }

            if (e.Cancel == false && e.TabPage != null && ViewSet.ViewsList.ContainsKey(e.TabPage.Name))
            {
                await CallShowView(e.TabPage);
            }
        }

        private async Task CallShowView(TabPage Page)
        {
            bool result = await ShowViewAsync(Page);

            if (!result)
            {
                await RemoveTabAsync(Page);
            }
        }

        private async void ViewsTabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (e.TabPage == null)
            {
                return;
            }

            _lastSelectedPage = e.TabPage;

            if (IsBusy)
            {
                e.Cancel = true;
                return;
            }

            // If forceDeselecting is set to one, return here to prevent the code from infinite loops
            if (_forceDeselecting)
            {
                _targetingPage = null;
                _forceDeselecting = false;
                return;
            }

            if (_targetingPage == null)
            {
                for (int i = 0; i < TabPages.Count; i++)
                {
                    if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
                    {
                        _targetingPage = TabPages[i];
                    }
                }
            }

            e.Cancel = true;

            if (ViewSet.ViewsList.ContainsKey(LastSelectedTab.Name))
            {
                var view = ViewSet.ViewsList[e.TabPage.Name];

                IsBusy = true;

                if ((await view.HideAsync()) == true)
                {
                    IsBusy = false;

                    _forceDeselecting = true;

                    if (_targetingPage == null)
                    {
                        DeselectTab(e.TabPage);
                    }
                    else
                    {
                        SelectTab(_targetingPage);
                    }

                    return;
                }

                IsBusy = false;
            }
            else
            {
                IsBusy = false;
                _forceDeselecting = true;
                if (_targetingPage != null)
                {
                    SelectTab(_targetingPage);
                }
            }
        }

        void TabShortCut_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            _targetingPage = TabPages[item.Name];
            SelectTab(item.Name);
        }

        private async void OnMenuClick_CloseAll(object sender, EventArgs e)
        {
            await RemoveAllAsync();
        }

        private async void OnMenuClick_CloseAllButThis(object sender, EventArgs e)
        {
            await RemoveAllAsync(_hoveredPage);
        }

        /// <summary>
        /// This event is getting called when user clicks on the tab.
        /// The menu shows entries for closing tab and for quick switching.
        /// </summary>
        private void MenuOpeningEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsBusy)
            {
                e.Cancel = true;
                return;
            }

            _hoveredPage = null;

            int items = ContextMenuStrip.Items.Count;

            for (int i = 0; i < items - menuIndexOffset; i++)
            {
                ContextMenuStrip.Items.RemoveAt(menuIndexOffset);
            }

            if (TabPages.Count > 1)
            {
                ContextMenuStrip.Items.Add(new ToolStripSeparator());
            }

            for (int i = 0; i < TabPages.Count; i++)
            {
                if (TabPages[i] != SelectedTab)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(TabPages[i].Text);

                    item.Name = TabPages[i].Name;
                    item.Click += TabShortCut_Click;

                    if (!string.IsNullOrEmpty(TabPages[i].ImageKey))
                    {
                        item.Image = ImageList.Images[TabPages[i].ImageKey];
                    }

                    ContextMenuStrip.Items.Add(item);
                }

                if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
                {
                    _hoveredPage = TabPages[i];
                }
            }

            if (_hoveredPage == null || NotCloseableTabs.Contains(_hoveredPage.Name))
            {
                e.Cancel = true;
            }
        }

        public async Task<bool> RemoveAllAsync()
        {
            return await RemoveAllAsync(null);
        }

        public async Task<bool> RemoveAllAsync(TabPage RemainingPage = null)
        {
            bool allClosed = true;

            IsBusy = true;
            _isBusyForce = true;

            for (int i = TabPages.Count - 1; i >= 0; i--)
            {
                TabPage Page = TabPages[i];

                if (Page == RemainingPage)
                {
                    continue;
                }

                if (!NotCloseableTabs.Contains(Page.Name))
                {
                    if ((await RemoveTabAsync(Page)) == false)
                    {
                        allClosed = false;
                    }
                }
            }

            _isBusyForce = false;
            IsBusy = false;

            if (RemainingPage != null)
            {
                SelectedTab = RemainingPage;
            }

            return allClosed;
        }

        private async void OnMenuClick_Close(object sender, EventArgs e)
        {
            await RemoveTabAsync(_hoveredPage);
        }

        private async void OnMenuClick_Refresh(object sender, EventArgs e)
        {
            await RefreshViewAsync(_hoveredPage);
        }

        public async Task<bool> RemoveTabAsync(TabPage Page)
        {
            if (ViewSet.ViewsList.ContainsKey(Page.Name))
            {
                bool alreadyBusy = false;

                var view = ViewSet.ViewsList[Page.Name];

                if (!IsBusy)
                {
                    IsBusy = true;
                }
                else
                {
                    alreadyBusy = true;
                }

                // Removing the page if it can be closed
                if ((await view.CloseAsync()) == true)
                {
                    IsBusy = false;
                    TabPages.Remove(Page);
                    return true;
                }
                else
                {
                    SelectTab(Page);
                    Logger.Log(string.Format("Tab {0} has canceled closing", Page.Name), 0, LogMessageType.Exclamation);
                }

                if (!alreadyBusy)
                {
                    IsBusy = false;
                }
            }

            return false;
        }

        public async Task RefreshOrCloseViews()
        {
            IsBusy = true;
            _isBusyForce = true;

            foreach (TabPage page in TabPages)
            {
                await RefreshViewAsync(page);
            }

            _isBusyForce = false;
            IsBusy = false;
        }

        private async Task RefreshViewAsync(TabPage page)
        {
            IsBusy = true;

            // Tab page shows not a view, so it can't be found
            if (ViewSet.ViewsList.ContainsKey(page.Name))
            {
                var view = ViewSet.ViewsList[page.Name];

                // Remove the tab if the view can't be shown
                if ((await view.IsEnabledAsync()) == false)
                {
                    await RemoveTabAsync(page);
                }
                else
                {
                    page.Text = view.Title;
                    await view.RefreshAsync();
                }
            }

            IsBusy = false;
        }

        public async Task AutoOpenViews(string Target)
        {
            bool firstDone = TabPages.Count > 0;

            foreach (KeyValuePair<string, DynView> entry in ViewSet.ViewsList)
            {
                if (entry.Value.AutoOpen && entry.Value.Target.ToLower() == Target.ToLower())
                {
                    await AddViewTab(entry.Value.Name, entry.Value.EntryPoint, !firstDone);
                    firstDone = true;
                }
            }
        }
    }
}
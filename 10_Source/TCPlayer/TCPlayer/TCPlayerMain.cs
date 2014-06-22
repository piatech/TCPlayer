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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPlayer.CustomControls;
using TCPlayer.Project;
using TCPlayer.API;
using System.Threading;
using TCPlayer.Forms;
using System.IO;

namespace TCPlayer
{
    public partial class TCPlayerMain : Form
    {
        internal static string _applicationName = "Target Control Player";

        private TCProject _project = new TCProject();

        // Is set if the windows should be closed after all async methods returned
        private bool _forceClose = false;

        private Logger _logger;

        // Is set to true if some async jobs are active
        private bool _isBusy = false;

        private StatusBarProgress _statusBarProgress;
        private ProgressDialog _dialogProgress;

        // Checks the connection when the player is connected to target
        private System.Timers.Timer _connectionMonitoring;

        // Timer for a cycle update
        private System.Timers.Timer _updateCycleTimer;
        private TextBoxStreamWriter _txtBoxStreamWriter;

        public TCPlayerMain()
        {
            InitializeComponent();

            this.Text = _applicationName;

            RedirectConsole();

            AlertBox.UIContext = SynchronizationContext.Current;
            AlertBox.OwnerForm = this;

            SetupLoggerObject();

            SetupProjectObject();

            _statusBarProgress = new StatusBarProgress(SynchronizationContext.Current);
            _statusBarProgress.StatusBarLabel = toolStripStatus;
            _statusBarProgress.StatusBarProgressBar = toolStripProgressBarBusy;

            // Initialize the connection monitoring timer
            _connectionMonitoring = new System.Timers.Timer(1000);
            _connectionMonitoring.Elapsed += _connectionMonitoring_Elapsed;

            // Timer for update cycle
            _updateCycleTimer = new System.Timers.Timer(1000);
            _updateCycleTimer.Elapsed += _updateCycleTimer_Elapsed;

            mainViewsTabControl.OnLockUI += ViewsTabControl_OnLockUI;
            subViewsTabControl.OnLockUI += ViewsTabControl_OnLockUI;

            subViewsTabControl.NotCloseableTabs.Add("tabPageLog");
            subViewsTabControl.NotCloseableTabs.Add("tabPageConsole");

            _dialogProgress = new ProgressDialog(SynchronizationContext.Current, this);
        }

        private void RedirectConsole()
        {
            _txtBoxStreamWriter = new TextBoxStreamWriter();
            _txtBoxStreamWriter.OutputTextBox = textBoxConsole;
            Console.SetOut(_txtBoxStreamWriter);
        }

        private void _updateCycleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _updateCycleTimer.Enabled = false;

            if (_project.IsLoaded)
            {
                _project.CallUpdateCycle();
            }
            
            _updateCycleTimer.Enabled = true;
        }

        private void SetupLoggerObject()
        {
            _logger = new Logger();
            _logger.LogList = loggerList;
            _logger.StatusBarLabel = toolStripStatus;
        }

        private void SetupProjectObject()
        {
            _project.UIContext = SynchronizationContext.Current;

            _project.ApplicationBasePath = Utils.GetApplicationBasePath();

            // Callback for components and views, so they can trigger the UI update
            _project.OnUINeedsUpdate += async delegate(bool ViewsAndUI)
            {
                if (ViewsAndUI)
                {
                    await UpdateViewsAndUI();
                }
                else
                {
                    UpdateUI();
                }
            };

            // Callback for opening a view
            _project.GoToView += delegate(string ViewName, string EntryPoint)
            {
                if (_project.ViewSet.ViewsList.ContainsKey(ViewName))
                {
                    // Execute the method in the UI thread
                    _project.UIContext.Send(async d =>
                    {
                        await LoadViewToTabControl(_project.ViewSet.ViewsList[ViewName], EntryPoint, true);
                    }, null);
                }
                else
                {
                    _logger.Log(string.Format("Can not open view '{0}'", ViewName), 0, LogMessageType.Failure);
                }
            };

            _project.MainLogger = _logger;
        }

        void _connectionMonitoring_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_project.IsOnline)
            {
                return;
            }

            // Stop updating the connection monitoring elapsed method
            _connectionMonitoring.Enabled = false;

            if (CheckConnectionWithTimeout(2000))
            {
                _connectionMonitoring.Enabled = true;
            }
            else
            {
                _connectionMonitoring.Stop();

                if (!_project.IsOnline)
                {
                    return;
                }

                _project.UIContext.Post(async s =>
                {
                    await GoOfflineAsync(_dialogProgress);

                    _logger.Log("Connection to target was closed!", 0, LogMessageType.Failure);
                }, null);
            }
        }

        private bool CheckConnectionWithTimeout(int timeoutInMilliseconds)
        {
            try
            {
                bool result = false;

                Utils.CallWithTimeOut(callAction: () =>
                {
                    result = _project.IsConnectedToTarget();
                },
                failAction: () =>
                {
                    result = false;
                },
                timeoutInMilliseconds: timeoutInMilliseconds);

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void ViewsTabControl_OnLockUI(bool locked)
        {
            LockUI(locked);
        }

        private async void TCPlayer_Load(object sender, EventArgs e)
        {
            RestoreWindowPosition();

            if (await LoadProject())
            {
                SaveProjectNameToRecentProjects();

                CreateUpdateCycleMenu();
            }
        }

        private void SaveProjectNameToRecentProjects()
        {
            try
            {
                MostRecentProjects mrp = new MostRecentProjects(Application.UserAppDataRegistry);
                mrp.AddProject(_project.GetProjectInfo("Title"), _project.GetProjectInfo("ProjectFilePath"));
                mrp.Save();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }
        }

        private void CreateUpdateCycleMenu()
        {
            int[] refreshRates = { 2000, 1000, 500 };

            foreach (int rate in refreshRates)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();

                item.Name = rate.ToString();

                if (rate >= 1000)
                {
                    item.Text = string.Format("{0} s", rate / 1000);
                }
                else
                {
                    item.Text = string.Format("{0} ms", rate);
                }

                item.Click += delegate(object _sender, EventArgs _e)
                {
                    _updateCycleTimer.Interval = rate;
                    CheckMenuItemForCycleRate();
                };

                updateRateToolStripMenuItem.DropDownItems.Add(item);
            }

            CheckMenuItemForCycleRate();
        }

        private void CheckMenuItemForCycleRate()
        {
            foreach (ToolStripMenuItem item in updateRateToolStripMenuItem.DropDownItems)
            {
                if (int.Parse(item.Name) == _updateCycleTimer.Interval)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }
        }

        private async Task<bool> LoadProject()
        {
            LockUI();

            try
            {
                string[] args = Environment.GetCommandLineArgs();
                string filePath = null;

                // If the project file provided via the command line, open it
                if (args.Count() > 1 && File.Exists(args[1]))
                {
                    filePath = args[1];
                }
                else
                {
                    ProjectSelectionDialog pSel = new ProjectSelectionDialog();

                    // If the project file was not found, show the open file dialog
                    if (pSel.ShowDialog() == DialogResult.OK)
                    {
                        filePath = pSel.FileName;
                    }
                    else
                    {
                        _isBusy = false;
                        Close();
                        return false;
                    }
                }

                this.Activate();

                // Load the project
                await _project.LoadAsync(FilePath: filePath, Progress: _statusBarProgress);

                mainViewsTabControl.ViewSet = _project.ViewSet;
                mainViewsTabControl.Logger = _logger;

                subViewsTabControl.ViewSet = _project.ViewSet;
                subViewsTabControl.Logger = _logger;

                // Creating the menu for target
                CreateTargetMenu();

                // Creating the menu for all components except target component
                CreateComponentsMenu(excludeComponents: new List<string>() { "Target" });

                // Getting the icons from views and add them to the icons list
                CreateTabIconsList();

                // Start the update cycle
                _updateCycleTimer.Start();
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                _isBusy = false;
                Close();
                return false;
            }

            UnlockUI();

            await UpdateViewsAndUI();

            LockUI();

            await AutoOpenAllViews();

            ShowMenuForCurrentView(mainViewsTabControl, mainViewMenuToolStripMenuItem);

            ShowMenuForCurrentView(subViewsTabControl, subViewToolStripMenuItem);

            UnlockUI();

            return true;
        }

        private async Task AutoOpenAllViews()
        {
            await mainViewsTabControl.AutoOpenViews("main");

            await subViewsTabControl.AutoOpenViews("sub");
        }

        private void CreateTabIconsList()
        {
            // Add all plugin's icons to the icons list
            foreach (KeyValuePair<string, DynView> entry in _project.ViewSet.ViewsList)
            {
                if (entry.Value.Icon != null)
                {
                    tabIcons.Images.Add(entry.Key, entry.Value.Icon);
                }
            }
        }

        private void CreateTargetMenu()
        {
            // Adding target menu items
            var targetMenuItems = _project.GetTargetMenuStripItems();

            if (targetMenuItems != null)
            {
                targetToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                targetToolStripMenuItem.DropDownItems.AddRange(targetMenuItems);
            }

            // Adding target toolbar items
            var targetToolStripItems = _project.GetTargetToolStripItems();

            if (targetToolStripItems != null)
            {
                mainToolStrip.Items.Add(new ToolStripSeparator());
                mainToolStrip.Items.AddRange(targetToolStripItems);
            }
        }

        private void CreateComponentsMenu(List<string> excludeComponents = null)
        {
            foreach (DynComponent component in _project.ComponentSet)
            {
                if (excludeComponents != null && excludeComponents.Contains(component.ComponentType))
                {
                    continue;
                }

                var menuItems = component.MenuItems;
                var toolStripItems = component.ToolStripItems;

                if (menuItems != null)
                {
                    ToolStripMenuItem componentMenu = new ToolStripMenuItem();
                    componentMenu.Text = component.Title;
                    componentMenu.DropDownItems.AddRange(menuItems);
                    mainMenuStrip.Items.Insert(mainMenuStrip.Items.Count - 1, componentMenu);
                }

                if (toolStripItems != null)
                {
                    mainToolStrip.Items.AddRange(toolStripItems);
                }
            }
        }

        private async Task BuildTreeAsync()
        {
            List<string> expandedNodeNames = CollectExpandedNodes(viewsTreeView.Nodes);

            // Save the name of the selected node
            string selectedName = (viewsTreeView.SelectedNode != null) ? viewsTreeView.SelectedNode.Name : "";

            // Remove all nodes
            viewsTreeView.Nodes.Clear();

            viewsTreeView.Nodes.Add("__loading__", "Loading...", "loadingView");

            var Nodes = await Task<TreeNode[]>.Factory.StartNew(() =>
            {
                return _project.GetTreeNodes().ToArray();
            });

            viewsTreeView.Nodes.Clear();

            // Rebuild the tree
            viewsTreeView.Nodes.AddRange(Nodes);

            // Expand the nodes which were expanded before
            foreach (string expandedNodeName in expandedNodeNames)
            {
                TreeNode[] expandedNode = viewsTreeView.Nodes.Find(expandedNodeName, true);

                if (expandedNode != null && expandedNode.Count() > 0)
                {
                    expandedNode.First().Expand();
                }
            }

            // Select the last selected node
            if (selectedName.Count() > 0)
            {
                TreeNode[] lastNode = viewsTreeView.Nodes.Find(selectedName, true);

                if (lastNode != null && lastNode.Count() > 0)
                {
                    lastNode.First().EnsureVisible();
                    viewsTreeView.SelectedNode = lastNode.First();
                }
            }
        }

        private List<string> CollectExpandedNodes(TreeNodeCollection nodes)
        {
            List<string> names = new List<string>();

            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded)
                {
                    names.Add(node.Name);

                    names.AddRange(CollectExpandedNodes(node.Nodes));
                }
            }

            return names;
        }

        private async void TCPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_forceClose || !_project.IsLoaded)
            {
                return;
            }

            if (_isBusy)
            {
                _logger.Log(string.Format(Resources.Messages.ProgramIsBusyPleaseWait, _applicationName),
                    0, LogMessageType.Information, LogReceiver.MessageBox);

                e.Cancel = true;
                return;
            }

            e.Cancel = true;

            bool result = await CloseProjectAsync();

            if (result)
            {
                SaveWindowPosition();

                // Setting forceClose to true, so we don't stuck in endless loop
                _forceClose = true;
                _txtBoxStreamWriter.OutputTextBox = null;
                Close();
            }
        }

        private void SaveWindowPosition()
        {
            try
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    Properties.Settings.Default.Location = RestoreBounds.Location;
                    Properties.Settings.Default.Size = RestoreBounds.Size;
                    Properties.Settings.Default.Maximized = true;
                    Properties.Settings.Default.Minimized = false;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.Location = Location;
                    Properties.Settings.Default.Size = Size;
                    Properties.Settings.Default.Maximized = false;
                    Properties.Settings.Default.Minimized = false;
                }
                else
                {
                    Properties.Settings.Default.Location = RestoreBounds.Location;
                    Properties.Settings.Default.Size = RestoreBounds.Size;
                    Properties.Settings.Default.Maximized = false;
                    Properties.Settings.Default.Minimized = true;
                }

                Properties.Settings.Default.SplitterMainSub = splitContainerMainSub.SplitterDistance;
                Properties.Settings.Default.SplitterLeftRight = splitContainerTreeContent.SplitterDistance;

                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {

            }
        }

        private void RestoreWindowPosition()
        {
            try
            {
                if (Properties.Settings.Default.Maximized)
                {
                    WindowState = FormWindowState.Maximized;
                }
                else if (Properties.Settings.Default.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                }

                if (Properties.Settings.Default.Size.Width > 0 && Properties.Settings.Default.Size.Height > 0)
                {
                    Location = Properties.Settings.Default.Location;
                    Size = Properties.Settings.Default.Size;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task<bool> CanCloseCurrentProject()
        {
            if (_project.IsLoaded && _project.HasChanges)
            {
                DialogResult result = MessageBox.Show(Resources.Messages.SaveProjectBeforeClosing,
                    _applicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.Yes:
                        return await SaveProjectAsync(_statusBarProgress);
                    case DialogResult.No:
                        break;
                }
            }

            return true;
        }

        private async Task<bool> SaveProjectAsync(IProgressEx Progress)
        {
            bool result;

            try
            {
                LockUI();

                result = await _project.SaveAsync(Progress);

                UnlockUI();
            }
            catch (Exception ex)
            {
                result = false;
                _logger.Log(ex.Message, 0, LogMessageType.Failure, LogReceiver.MessageBox | LogReceiver.Console);
            }

            UpdateUI();

            return result;
        }

        private async Task<bool> CloseProjectAsync()
        {
            if (_project.IsLoaded)
            {
                if (_project.IsOnline)
                {
                    if ((await GoOfflineAsync(_dialogProgress)) == false)
                    {
                        _logger.Log(Resources.Messages.CanNotCloseTheProject, 0, LogMessageType.Exclamation, LogReceiver.Console);
                        return false;
                    }
                }
            }

            if ((await CanCloseCurrentProject()) == false)
            {
                return false;
            }

            if (_project.IsLoaded)
            {
                if ((await CloseTabsAsync()) == false)
                {
                    return false;
                }

                _project.Unload();
            }

            viewsTreeView.Nodes.Clear();

            UpdateUI();

            return true;
        }

        private async Task<bool> CloseTabsAsync()
        {
            LockUI();

            if ((await mainViewsTabControl.RemoveAllAsync()) == false)
            {
                UnlockUI();
                return false;
            }

            if ((await subViewsTabControl.RemoveAllAsync()) == false)
            {
                UnlockUI();
                return false;
            }

            UnlockUI();
            return true;
        }

        private async void toolStripButtonSaveProject_Click(object sender, EventArgs e)
        {
            await SaveProjectAsync(_statusBarProgress);
        }

        private async void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            await GoOnlineAsync();
        }

        private async void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            await GoOfflineAsync(_dialogProgress);
        }

        private async Task<bool> GoOnlineAsync()
        {
            LockUI();

            _logger.Log(Resources.Messages.GoingOnline, 0,
                LogMessageType.Information, LogReceiver.StatusBar | LogReceiver.Console);

            if ((await _project.GoOnlineAsync(Progress: _dialogProgress) == false))
            {
                _logger.Log(Resources.Messages.ProjectCanNotGoOnline, 0, LogMessageType.Exclamation, LogReceiver.Console | LogReceiver.StatusBar);
                UnlockUI();
                return false;
            }

            UnlockUI();

            _logger.Log(Resources.Messages.ProjectIsOnline, 0, LogMessageType.Information, LogReceiver.StatusBar);

            await UpdateViewsAndUI();

            StartConnectionMonitoring();

            return true;
        }

        private void StartConnectionMonitoring()
        {
            _connectionMonitoring.Start();
        }

        private void StopConnectionMonitoring()
        {
            _connectionMonitoring.Stop();
        }

        private async Task<bool> GoOfflineAsync(IProgressEx Progress)
        {
            LockUI();

            StopConnectionMonitoring();

            if ((await _project.GoOfflineAsync(Progress) == false))
            {
                _logger.Log(Resources.Messages.ProjectCanNotGoOffline, 0, LogMessageType.Exclamation, LogReceiver.Console);
                UnlockUI();
                StartConnectionMonitoring();
                return false;
            }

            UnlockUI();

            await UpdateViewsAndUI();

            return true;
        }

        private void toolStripButtonDownload_Click(object sender, EventArgs e)
        {

        }

        private async Task LoadViewToTabControl(DynView view, string EntryPoint, bool ShowView = false)
        {
            // Get the tab control for the target
            ViewsTabControl TabPanel = GetTabControlForTarget(view.Target);

            await TabPanel.AddViewTab(view.Name, EntryPoint, ShowView);
        }

        private ViewsTabControl GetTabControlForTarget(string TargetPanel)
        {
            ViewsTabControl TabPanel = null;

            switch (TargetPanel.ToLower())
            {
                case "sub":
                    TabPanel = subViewsTabControl;
                    break;
                default:
                    TabPanel = mainViewsTabControl;
                    break;
            }

            return TabPanel;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SaveProjectAsync(_statusBarProgress);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonConnect_Click(sender, e);
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonDisconnect_Click(sender, e);
        }

        private void viewsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private async void viewsTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var hitTest = ((TreeView)sender).HitTest(e.Location);

                if (hitTest.Location == TreeViewHitTestLocations.Label || hitTest.Location == TreeViewHitTestLocations.Image)
                {
                    if (_project.ViewSet.ViewsList.ContainsKey(e.Node.Name))
                    {
                        // Display the selected view
                        await LoadViewToTabControl(_project.ViewSet.ViewsList[e.Node.Name],
                            _project.ViewSet.ViewsList[e.Node.Name].EntryPoint, true);
                        e.Node.Expand();
                    }
                }
            }
        }

        private void viewsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private async Task UpdateViewsAndUI()
        {
            if (_isBusy)
            {
                return;
            }

            LockUI();

            _logger.Log(Resources.Messages.UpdatingViews, 0, LogMessageType.Information, LogReceiver.StatusBar);

            // Rebuild the views tree
            await BuildTreeAsync();

            // Refresh main tabs
            await mainViewsTabControl.RefreshOrCloseViews();

            // Refresh sub tabs
            await subViewsTabControl.RefreshOrCloseViews();

            bool showProjectTree = _project.GetProperty<bool>("ShowProjectTree", true);
            bool showLog = _project.GetProperty<bool>("ShowLog", true);

            projectTreeToolStripMenuItem.Checked = showProjectTree;
            logToolStripMenuItem.Checked = showLog;

            splitContainerTreeContent.Panel1Collapsed = !showProjectTree;
            splitContainerMainSub.Panel2Collapsed = !showLog;

            tabPageProject.Text = _project.GetProjectInfo("Title");

            _logger.Ready();

            UnlockUI();
        }

        private void UpdateTitle()
        {
            string title;
            string changesIndicator = "";

            if (_project.HasChanges)
            {
                changesIndicator = "*";
            }

            if (_project.IsLoaded)
            {
                string onlineIndicator = "";

                if (_project.IsOnline)
                {
                    onlineIndicator = " [" + Resources.Messages.Online + "]";
                }

                _applicationName = _project.Properties["Title"].ToString();

                title = string.Format("{0}{1}{2}", _applicationName, changesIndicator, onlineIndicator);
            }
            else
            {
                title = string.Format("{0}{1}", _applicationName, changesIndicator);
            }

            Invoke(new Action(() => this.Text = title));
        }

        private void UpdateUI()
        {
            UpdateTitle();
            UpdateMainMenuStrip();
            UpdateToolStrip();
            UpdateStatusStrip();
        }

        private void LockUI(bool locked = true)
        {
            _isBusy = locked;

            if (_isBusy)
            {
                _statusBarProgress.ShowBusyState();
            }
            else
            {
                _statusBarProgress.Done();
            }

            mainToolStrip.Enabled = !locked;

            mainMenuStrip.Enabled = !locked;

            if (locked)
            {
                Application.DoEvents();
            }
            else
            {
                Cursor.Position = Cursor.Position;
                UpdateUI();
            }
        }

        private void UnlockUI()
        {
            LockUI(false);
        }

        private void UpdateStatusStrip()
        {
            if (_project.IsLoaded)
            {
                if (_project.IsOnline)
                {
                    // Showing the "Online" message
                    statusOnlineLabel.ForeColor = Color.Green;
                    statusOnlineLabel.Text = Resources.Messages.Online;

                    // Changing the background image to stripes
                    mainStatusStrip.BackgroundImage = Resources.Resources.stripe;

                    // Displaying the targets addresses string representation
                    toolStripStatusTarget.Visible = true;
                    toolStripStatusTarget.Text = string.Format(Resources.Messages.ConnectedTo, _project.TargetsFullAddress);
                    statusOnlineLabel.Visible = false;
                }
                else
                {
                    statusOnlineLabel.Visible = true;
                    statusOnlineLabel.ForeColor = Color.OrangeRed;
                    statusOnlineLabel.Text = Resources.Messages.Offline;
                    mainStatusStrip.BackgroundImage = null;
                    toolStripStatusTarget.Visible = false;
                }
            }
        }

        private void UpdateToolStrip()
        {
            if (_project.IsLoaded)
            {
                toolStripButtonSaveProject.Enabled = _project.HasChanges && !_project.IsOnline;

                toolStripButtonConnect.Enabled = !_project.IsOnline;
                toolStripButtonDisconnect.Enabled = _project.IsOnline;
            }
        }

        private void UpdateMainMenuStrip()
        {
            if (_project.IsLoaded)
            {
                saveProjectToolStripMenuItem.Enabled = _project.HasChanges && !_project.IsOnline;

                connectToolStripMenuItem.Enabled = !_project.IsOnline;

                disconnectToolStripMenuItem.Enabled = _project.IsOnline;
            }
        }

        private void projectTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerTreeContent.Panel1Collapsed = !splitContainerTreeContent.Panel1Collapsed;
            projectTreeToolStripMenuItem.Checked = !splitContainerTreeContent.Panel1Collapsed;
            _project.SetProperty("ShowProjectTree", (!splitContainerTreeContent.Panel1Collapsed).ToString());
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainerMainSub.Panel2Collapsed = !splitContainerMainSub.Panel2Collapsed;
            logToolStripMenuItem.Checked = !splitContainerMainSub.Panel2Collapsed;
            _project.SetProperty("ShowLog", (!splitContainerMainSub.Panel2Collapsed).ToString());
        }

        private async void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateViewsAndUI();
        }

        private void aboutTCPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            about.ShowDialog();
        }

        private void logContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolStripMenuItemClearLog_Click(object sender, EventArgs e)
        {
            _logger.ClearConsole();
        }

        private void componentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComponentsListDialog cld = new ComponentsListDialog();
            cld.Project = _project;
            cld.ShowDialog();
        }

        private void viewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewsListDialog vld = new ViewsListDialog();
            vld.Project = _project;
            vld.ShowDialog();
        }

        /// <summary>
        /// Displays a menu for the currently selected view tab
        /// </summary>
        /// <param name="Control">TabControl for the view</param>
        /// <param name="Menu">Menu item in the main menu strip</param>
        private void ShowMenuForCurrentView(ViewsTabControl Control, ToolStripMenuItem Menu)
        {
            try
            {
                DynView view = Control.CurrentView;

                if(view != null && view.MenuItems != null)
                {
                    Menu.Text = view.Title;
                    Menu.DropDownItems.Clear();
                    Menu.DropDownItems.AddRange(view.MenuItems);
                    Menu.Visible = true;
                }
                else
                {
                    Menu.Visible = false;
                }
            }
            catch(Exception)
            {
                Menu.Visible = false;
            }
        }

        private void mainViewsTabControl_Selected(object sender, TabControlEventArgs e)
        {
            ShowMenuForCurrentView((ViewsTabControl)sender, mainViewMenuToolStripMenuItem);
        }

        private void subViewsTabControl_Selected(object sender, TabControlEventArgs e)
        {
            ShowMenuForCurrentView((ViewsTabControl)sender, subViewToolStripMenuItem);
        }

        private void toolStripMenuItemClearConsole_Click(object sender, EventArgs e)
        {
            textBoxConsole.Clear();
        }

        private async void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSettings dialog = new ProjectSettings(_project);

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                await UpdateViewsAndUI();
            }
        }
    }
}
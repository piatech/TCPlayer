namespace TCPlayer
{
    partial class TCPlayerMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPlayerMain));
            this.splitContainerMainSub = new System.Windows.Forms.SplitContainer();
            this.splitContainerTreeContent = new System.Windows.Forms.SplitContainer();
            this.tabControlProject = new System.Windows.Forms.TabControl();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.viewsTreeView = new System.Windows.Forms.TreeView();
            this.tabIcons = new System.Windows.Forms.ImageList(this.components);
            this.mainViewsTabControl = new TCPlayer.CustomControls.ViewsTabControl();
            this.subViewsTabControl = new TCPlayer.CustomControls.ViewsTabControl();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.loggerList = new System.Windows.Forms.ListView();
            this.Level = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MessageId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.loggerImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabPageConsole = new System.Windows.Forms.TabPage();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.consoleContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemClearConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.projectSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.projectTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainViewMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.componentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutTCPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSaveProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDisconnect = new System.Windows.Forms.ToolStripButton();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarBusy = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusOnlineLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusTarget = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainSub)).BeginInit();
            this.splitContainerMainSub.Panel1.SuspendLayout();
            this.splitContainerMainSub.Panel2.SuspendLayout();
            this.splitContainerMainSub.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeContent)).BeginInit();
            this.splitContainerTreeContent.Panel1.SuspendLayout();
            this.splitContainerTreeContent.Panel2.SuspendLayout();
            this.splitContainerTreeContent.SuspendLayout();
            this.tabControlProject.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.subViewsTabControl.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.logContextMenu.SuspendLayout();
            this.tabPageConsole.SuspendLayout();
            this.consoleContextMenu.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMainSub
            // 
            resources.ApplyResources(this.splitContainerMainSub, "splitContainerMainSub");
            this.splitContainerMainSub.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMainSub.Name = "splitContainerMainSub";
            // 
            // splitContainerMainSub.Panel1
            // 
            this.splitContainerMainSub.Panel1.Controls.Add(this.splitContainerTreeContent);
            // 
            // splitContainerMainSub.Panel2
            // 
            this.splitContainerMainSub.Panel2.Controls.Add(this.subViewsTabControl);
            // 
            // splitContainerTreeContent
            // 
            resources.ApplyResources(this.splitContainerTreeContent, "splitContainerTreeContent");
            this.splitContainerTreeContent.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerTreeContent.Name = "splitContainerTreeContent";
            // 
            // splitContainerTreeContent.Panel1
            // 
            this.splitContainerTreeContent.Panel1.Controls.Add(this.tabControlProject);
            // 
            // splitContainerTreeContent.Panel2
            // 
            this.splitContainerTreeContent.Panel2.Controls.Add(this.mainViewsTabControl);
            // 
            // tabControlProject
            // 
            this.tabControlProject.Controls.Add(this.tabPageProject);
            resources.ApplyResources(this.tabControlProject, "tabControlProject");
            this.tabControlProject.ImageList = this.tabIcons;
            this.tabControlProject.Name = "tabControlProject";
            this.tabControlProject.SelectedIndex = 0;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.viewsTreeView);
            resources.ApplyResources(this.tabPageProject, "tabPageProject");
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // viewsTreeView
            // 
            this.viewsTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.viewsTreeView, "viewsTreeView");
            this.viewsTreeView.ImageList = this.tabIcons;
            this.viewsTreeView.Name = "viewsTreeView";
            this.viewsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.viewsTreeView_AfterSelect);
            this.viewsTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.viewsTreeView_NodeMouseClick);
            this.viewsTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.viewsTreeView_NodeMouseDoubleClick);
            // 
            // tabIcons
            // 
            this.tabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tabIcons.ImageStream")));
            this.tabIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.tabIcons.Images.SetKeyName(0, "arrow");
            this.tabIcons.Images.SetKeyName(1, "plugin.png");
            this.tabIcons.Images.SetKeyName(2, "tabPageProject");
            this.tabIcons.Images.SetKeyName(3, "loadingView");
            this.tabIcons.Images.SetKeyName(4, "noIcon");
            this.tabIcons.Images.SetKeyName(5, "tabPageLog");
            this.tabIcons.Images.SetKeyName(6, "tabPageConsole");
            // 
            // mainViewsTabControl
            // 
            resources.ApplyResources(this.mainViewsTabControl, "mainViewsTabControl");
            this.mainViewsTabControl.ImageList = this.tabIcons;
            this.mainViewsTabControl.LastSelectedTab = null;
            this.mainViewsTabControl.Logger = null;
            this.mainViewsTabControl.Name = "mainViewsTabControl";
            this.mainViewsTabControl.NotCloseableTabs = ((System.Collections.Generic.List<string>)(resources.GetObject("mainViewsTabControl.NotCloseableTabs")));
            this.mainViewsTabControl.SelectedIndex = 0;
            this.mainViewsTabControl.ViewSet = null;
            this.mainViewsTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.mainViewsTabControl_Selected);
            // 
            // subViewsTabControl
            // 
            this.subViewsTabControl.Controls.Add(this.tabPageLog);
            this.subViewsTabControl.Controls.Add(this.tabPageConsole);
            resources.ApplyResources(this.subViewsTabControl, "subViewsTabControl");
            this.subViewsTabControl.ImageList = this.tabIcons;
            this.subViewsTabControl.LastSelectedTab = null;
            this.subViewsTabControl.Logger = null;
            this.subViewsTabControl.Name = "subViewsTabControl";
            this.subViewsTabControl.NotCloseableTabs = ((System.Collections.Generic.List<string>)(resources.GetObject("subViewsTabControl.NotCloseableTabs")));
            this.subViewsTabControl.SelectedIndex = 0;
            this.subViewsTabControl.ViewSet = null;
            this.subViewsTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.subViewsTabControl_Selected);
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.loggerList);
            resources.ApplyResources(this.tabPageLog, "tabPageLog");
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // loggerList
            // 
            this.loggerList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Level,
            this.Message,
            this.MessageId,
            this.Time});
            this.loggerList.ContextMenuStrip = this.logContextMenu;
            resources.ApplyResources(this.loggerList, "loggerList");
            this.loggerList.FullRowSelect = true;
            this.loggerList.Name = "loggerList";
            this.loggerList.SmallImageList = this.loggerImageList;
            this.loggerList.UseCompatibleStateImageBehavior = false;
            this.loggerList.View = System.Windows.Forms.View.Details;
            // 
            // Level
            // 
            resources.ApplyResources(this.Level, "Level");
            // 
            // Message
            // 
            resources.ApplyResources(this.Message, "Message");
            // 
            // MessageId
            // 
            resources.ApplyResources(this.MessageId, "MessageId");
            // 
            // Time
            // 
            resources.ApplyResources(this.Time, "Time");
            // 
            // logContextMenu
            // 
            this.logContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClearLog});
            this.logContextMenu.Name = "logContextMenu";
            resources.ApplyResources(this.logContextMenu, "logContextMenu");
            this.logContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.logContextMenu_Opening);
            // 
            // toolStripMenuItemClearLog
            // 
            this.toolStripMenuItemClearLog.Image = global::TCPlayer.Properties.Resources.bin_empty;
            this.toolStripMenuItemClearLog.Name = "toolStripMenuItemClearLog";
            resources.ApplyResources(this.toolStripMenuItemClearLog, "toolStripMenuItemClearLog");
            this.toolStripMenuItemClearLog.Click += new System.EventHandler(this.toolStripMenuItemClearLog_Click);
            // 
            // loggerImageList
            // 
            this.loggerImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("loggerImageList.ImageStream")));
            this.loggerImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.loggerImageList.Images.SetKeyName(0, "failure");
            this.loggerImageList.Images.SetKeyName(1, "information");
            this.loggerImageList.Images.SetKeyName(2, "warning");
            this.loggerImageList.Images.SetKeyName(3, "exclamation");
            this.loggerImageList.Images.SetKeyName(4, "success");
            // 
            // tabPageConsole
            // 
            this.tabPageConsole.Controls.Add(this.textBoxConsole);
            resources.ApplyResources(this.tabPageConsole, "tabPageConsole");
            this.tabPageConsole.Name = "tabPageConsole";
            this.tabPageConsole.UseVisualStyleBackColor = true;
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.ContextMenuStrip = this.consoleContextMenu;
            resources.ApplyResources(this.textBoxConsole, "textBoxConsole");
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            // 
            // consoleContextMenu
            // 
            this.consoleContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemClearConsole});
            this.consoleContextMenu.Name = "consoleContextMenu";
            resources.ApplyResources(this.consoleContextMenu, "consoleContextMenu");
            // 
            // toolStripMenuItemClearConsole
            // 
            this.toolStripMenuItemClearConsole.Image = global::TCPlayer.Properties.Resources.bin_empty;
            this.toolStripMenuItemClearConsole.Name = "toolStripMenuItemClearConsole";
            resources.ApplyResources(this.toolStripMenuItemClearConsole, "toolStripMenuItemClearConsole");
            this.toolStripMenuItemClearConsole.Click += new System.EventHandler(this.toolStripMenuItemClearConsole_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.targetToolStripMenuItem,
            this.mainViewMenuToolStripMenuItem,
            this.subViewToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
            this.mainMenuStrip.Name = "mainMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProjectToolStripMenuItem,
            this.toolStripSeparator2,
            this.projectSettingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.disk2;
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            resources.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // projectSettingsToolStripMenuItem
            // 
            this.projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            resources.ApplyResources(this.projectSettingsToolStripMenuItem, "projectSettingsToolStripMenuItem");
            this.projectSettingsToolStripMenuItem.Click += new System.EventHandler(this.projectSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.updateRateToolStripMenuItem,
            this.toolStripSeparator5,
            this.projectTreeToolStripMenuItem,
            this.logToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // refreshToolStripMenuItem
            // 
            resources.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // updateRateToolStripMenuItem
            // 
            this.updateRateToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.time;
            this.updateRateToolStripMenuItem.Name = "updateRateToolStripMenuItem";
            resources.ApplyResources(this.updateRateToolStripMenuItem, "updateRateToolStripMenuItem");
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // projectTreeToolStripMenuItem
            // 
            this.projectTreeToolStripMenuItem.Name = "projectTreeToolStripMenuItem";
            resources.ApplyResources(this.projectTreeToolStripMenuItem, "projectTreeToolStripMenuItem");
            this.projectTreeToolStripMenuItem.Click += new System.EventHandler(this.projectTreeToolStripMenuItem_Click);
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            resources.ApplyResources(this.logToolStripMenuItem, "logToolStripMenuItem");
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // targetToolStripMenuItem
            // 
            this.targetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem});
            this.targetToolStripMenuItem.Name = "targetToolStripMenuItem";
            resources.ApplyResources(this.targetToolStripMenuItem, "targetToolStripMenuItem");
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.connect1;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            resources.ApplyResources(this.connectToolStripMenuItem, "connectToolStripMenuItem");
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            resources.ApplyResources(this.disconnectToolStripMenuItem, "disconnectToolStripMenuItem");
            this.disconnectToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.disconnect1;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // mainViewMenuToolStripMenuItem
            // 
            this.mainViewMenuToolStripMenuItem.Name = "mainViewMenuToolStripMenuItem";
            resources.ApplyResources(this.mainViewMenuToolStripMenuItem, "mainViewMenuToolStripMenuItem");
            // 
            // subViewToolStripMenuItem
            // 
            this.subViewToolStripMenuItem.Name = "subViewToolStripMenuItem";
            resources.ApplyResources(this.subViewToolStripMenuItem, "subViewToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.componentsToolStripMenuItem,
            this.viewsToolStripMenuItem,
            this.toolStripSeparator4,
            this.aboutTCPlayerToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // componentsToolStripMenuItem
            // 
            this.componentsToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.plugin;
            this.componentsToolStripMenuItem.Name = "componentsToolStripMenuItem";
            resources.ApplyResources(this.componentsToolStripMenuItem, "componentsToolStripMenuItem");
            this.componentsToolStripMenuItem.Click += new System.EventHandler(this.componentsToolStripMenuItem_Click);
            // 
            // viewsToolStripMenuItem
            // 
            this.viewsToolStripMenuItem.Image = global::TCPlayer.Properties.Resources.application_view_gallery;
            this.viewsToolStripMenuItem.Name = "viewsToolStripMenuItem";
            resources.ApplyResources(this.viewsToolStripMenuItem, "viewsToolStripMenuItem");
            this.viewsToolStripMenuItem.Click += new System.EventHandler(this.viewsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // aboutTCPlayerToolStripMenuItem
            // 
            this.aboutTCPlayerToolStripMenuItem.Name = "aboutTCPlayerToolStripMenuItem";
            resources.ApplyResources(this.aboutTCPlayerToolStripMenuItem, "aboutTCPlayerToolStripMenuItem");
            this.aboutTCPlayerToolStripMenuItem.Click += new System.EventHandler(this.aboutTCPlayerToolStripMenuItem_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSaveProject,
            this.toolStripSeparator3,
            this.toolStripButtonConnect,
            this.toolStripButtonDisconnect});
            resources.ApplyResources(this.mainToolStrip, "mainToolStrip");
            this.mainToolStrip.Name = "mainToolStrip";
            // 
            // toolStripButtonSaveProject
            // 
            this.toolStripButtonSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveProject.Image = global::TCPlayer.Properties.Resources.disk2;
            resources.ApplyResources(this.toolStripButtonSaveProject, "toolStripButtonSaveProject");
            this.toolStripButtonSaveProject.Name = "toolStripButtonSaveProject";
            this.toolStripButtonSaveProject.Click += new System.EventHandler(this.toolStripButtonSaveProject_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripButtonConnect
            // 
            this.toolStripButtonConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonConnect, "toolStripButtonConnect");
            this.toolStripButtonConnect.Name = "toolStripButtonConnect";
            this.toolStripButtonConnect.Click += new System.EventHandler(this.toolStripButtonConnect_Click);
            // 
            // toolStripButtonDisconnect
            // 
            this.toolStripButtonDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonDisconnect, "toolStripButtonDisconnect");
            this.toolStripButtonDisconnect.Name = "toolStripButtonDisconnect";
            this.toolStripButtonDisconnect.Click += new System.EventHandler(this.toolStripButtonDisconnect_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.toolStripProgressBarBusy,
            this.toolStripStatusLabel1,
            this.statusOnlineLabel,
            this.toolStripStatusTarget});
            resources.ApplyResources(this.mainStatusStrip, "mainStatusStrip");
            this.mainStatusStrip.Name = "mainStatusStrip";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            resources.ApplyResources(this.toolStripStatus, "toolStripStatus");
            this.toolStripStatus.Spring = true;
            // 
            // toolStripProgressBarBusy
            // 
            this.toolStripProgressBarBusy.Name = "toolStripProgressBarBusy";
            resources.ApplyResources(this.toolStripProgressBarBusy, "toolStripProgressBarBusy");
            this.toolStripProgressBarBusy.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // statusOnlineLabel
            // 
            resources.ApplyResources(this.statusOnlineLabel, "statusOnlineLabel");
            this.statusOnlineLabel.ForeColor = System.Drawing.Color.OrangeRed;
            this.statusOnlineLabel.Image = global::TCPlayer.Properties.Resources.disconnect;
            this.statusOnlineLabel.Name = "statusOnlineLabel";
            // 
            // toolStripStatusTarget
            // 
            this.toolStripStatusTarget.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.toolStripStatusTarget, "toolStripStatusTarget");
            this.toolStripStatusTarget.ForeColor = System.Drawing.Color.Green;
            this.toolStripStatusTarget.Image = global::TCPlayer.Properties.Resources.connect;
            this.toolStripStatusTarget.Name = "toolStripStatusTarget";
            // 
            // TCPlayerMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.splitContainerMainSub);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "TCPlayerMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPlayer_FormClosing);
            this.Load += new System.EventHandler(this.TCPlayer_Load);
            this.splitContainerMainSub.Panel1.ResumeLayout(false);
            this.splitContainerMainSub.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainSub)).EndInit();
            this.splitContainerMainSub.ResumeLayout(false);
            this.splitContainerTreeContent.Panel1.ResumeLayout(false);
            this.splitContainerTreeContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeContent)).EndInit();
            this.splitContainerTreeContent.ResumeLayout(false);
            this.tabControlProject.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.subViewsTabControl.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.logContextMenu.ResumeLayout(false);
            this.tabPageConsole.ResumeLayout(false);
            this.tabPageConsole.PerformLayout();
            this.consoleContextMenu.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMainSub;
        private System.Windows.Forms.SplitContainer splitContainerTreeContent;
        private CustomControls.ViewsTabControl mainViewsTabControl;
        private CustomControls.ViewsTabControl subViewsTabControl;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.ListView loggerList;
        private System.Windows.Forms.ColumnHeader Level;
        private System.Windows.Forms.ColumnHeader Message;
        private System.Windows.Forms.ColumnHeader MessageId;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveProject;
        private System.Windows.Forms.ToolStripButton toolStripButtonConnect;
        private System.Windows.Forms.ToolStripButton toolStripButtonDisconnect;
        private System.Windows.Forms.ImageList tabIcons;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusOnlineLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTarget;
        private System.Windows.Forms.ImageList loggerImageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarBusy;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabControl tabControlProject;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TreeView viewsTreeView;
        private System.Windows.Forms.ToolStripMenuItem componentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem aboutTCPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem updateRateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip logContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearLog;
        private System.Windows.Forms.ToolStripMenuItem viewsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainViewMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subViewToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageConsole;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.ContextMenuStrip consoleContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClearConsole;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem projectSettingsToolStripMenuItem;
    }
}


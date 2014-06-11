namespace TCPlayer.Forms
{
    partial class AlertBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertBox));
            this.okButton = new System.Windows.Forms.Button();
            this.fullMessageOutput = new System.Windows.Forms.TextBox();
            this.messageOutput = new System.Windows.Forms.TextBox();
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.copyLabelLink = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picturePanel = new System.Windows.Forms.Panel();
            this.detailsButton = new System.Windows.Forms.Button();
            this.detailsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // fullMessageOutput
            // 
            resources.ApplyResources(this.fullMessageOutput, "fullMessageOutput");
            this.fullMessageOutput.BackColor = System.Drawing.SystemColors.Window;
            this.fullMessageOutput.Name = "fullMessageOutput";
            this.fullMessageOutput.ReadOnly = true;
            // 
            // messageOutput
            // 
            this.messageOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.messageOutput, "messageOutput");
            this.messageOutput.Name = "messageOutput";
            this.messageOutput.ReadOnly = true;
            // 
            // detailsPanel
            // 
            this.detailsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.detailsPanel.Controls.Add(this.copyLabelLink);
            this.detailsPanel.Controls.Add(this.fullMessageOutput);
            resources.ApplyResources(this.detailsPanel, "detailsPanel");
            this.detailsPanel.Name = "detailsPanel";
            // 
            // copyLabelLink
            // 
            resources.ApplyResources(this.copyLabelLink, "copyLabelLink");
            this.copyLabelLink.Name = "copyLabelLink";
            this.copyLabelLink.TabStop = true;
            this.copyLabelLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.copyLabelLink_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.picturePanel);
            this.panel1.Controls.Add(this.detailsButton);
            this.panel1.Controls.Add(this.messageOutput);
            this.panel1.Controls.Add(this.okButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // picturePanel
            // 
            resources.ApplyResources(this.picturePanel, "picturePanel");
            this.picturePanel.Name = "picturePanel";
            // 
            // detailsButton
            // 
            resources.ApplyResources(this.detailsButton, "detailsButton");
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // AlertBox
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.Controls.Add(this.detailsPanel);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.AlertBox_Load);
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox fullMessageOutput;
        private System.Windows.Forms.TextBox messageOutput;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Panel picturePanel;
        private System.Windows.Forms.LinkLabel copyLabelLink;
    }
}
namespace TCPlayer.Forms
{
    partial class ProjectSettings
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageAppearance = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxProjectTree = new System.Windows.Forms.CheckBox();
            this.checkBoxShowLog = new System.Windows.Forms.CheckBox();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxVerboseLoad = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPageDescription.SuspendLayout();
            this.tabPageAppearance.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageDescription);
            this.tabControl1.Controls.Add(this.tabPageAppearance);
            this.tabControl1.Controls.Add(this.tabPageAdvanced);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(576, 326);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.textBoxVersion);
            this.tabPageDescription.Controls.Add(this.label4);
            this.tabPageDescription.Controls.Add(this.textBoxDescription);
            this.tabPageDescription.Controls.Add(this.label3);
            this.tabPageDescription.Controls.Add(this.textBoxAuthor);
            this.tabPageDescription.Controls.Add(this.label2);
            this.tabPageDescription.Controls.Add(this.textBoxTitle);
            this.tabPageDescription.Controls.Add(this.label1);
            this.tabPageDescription.Location = new System.Drawing.Point(4, 22);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageDescription.Size = new System.Drawing.Size(568, 300);
            this.tabPageDescription.TabIndex = 0;
            this.tabPageDescription.Text = "Description";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.Location = new System.Drawing.Point(13, 112);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(232, 20);
            this.textBoxVersion.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Project version:";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(277, 28);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(272, 104);
            this.textBoxDescription.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Project description:";
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Location = new System.Drawing.Point(13, 70);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(232, 20);
            this.textBoxAuthor.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Project author:";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(13, 28);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(232, 20);
            this.textBoxTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project title:";
            // 
            // tabPageAppearance
            // 
            this.tabPageAppearance.Controls.Add(this.groupBox2);
            this.tabPageAppearance.Controls.Add(this.groupBox1);
            this.tabPageAppearance.Location = new System.Drawing.Point(4, 22);
            this.tabPageAppearance.Name = "tabPageAppearance";
            this.tabPageAppearance.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageAppearance.Size = new System.Drawing.Size(568, 300);
            this.tabPageAppearance.TabIndex = 1;
            this.tabPageAppearance.Text = "Appearance";
            this.tabPageAppearance.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(13, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(344, 274);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Window";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxProjectTree);
            this.groupBox1.Controls.Add(this.checkBoxShowLog);
            this.groupBox1.Location = new System.Drawing.Point(372, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(183, 79);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Panels";
            // 
            // checkBoxProjectTree
            // 
            this.checkBoxProjectTree.AutoSize = true;
            this.checkBoxProjectTree.Location = new System.Drawing.Point(13, 26);
            this.checkBoxProjectTree.Name = "checkBoxProjectTree";
            this.checkBoxProjectTree.Size = new System.Drawing.Size(114, 17);
            this.checkBoxProjectTree.TabIndex = 0;
            this.checkBoxProjectTree.Text = "Show Project Tree";
            this.checkBoxProjectTree.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowLog
            // 
            this.checkBoxShowLog.AutoSize = true;
            this.checkBoxShowLog.Location = new System.Drawing.Point(13, 49);
            this.checkBoxShowLog.Name = "checkBoxShowLog";
            this.checkBoxShowLog.Size = new System.Drawing.Size(74, 17);
            this.checkBoxShowLog.TabIndex = 0;
            this.checkBoxShowLog.Text = "Show Log";
            this.checkBoxShowLog.UseVisualStyleBackColor = true;
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.groupBox3);
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageAdvanced.Size = new System.Drawing.Size(568, 300);
            this.tabPageAdvanced.TabIndex = 2;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(512, 344);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(431, 344);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxVerboseLoad);
            this.groupBox3.Location = new System.Drawing.Point(13, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox3.Size = new System.Drawing.Size(296, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debug";
            // 
            // checkBoxVerboseLoad
            // 
            this.checkBoxVerboseLoad.AutoSize = true;
            this.checkBoxVerboseLoad.Location = new System.Drawing.Point(13, 26);
            this.checkBoxVerboseLoad.Name = "checkBoxVerboseLoad";
            this.checkBoxVerboseLoad.Size = new System.Drawing.Size(88, 17);
            this.checkBoxVerboseLoad.TabIndex = 0;
            this.checkBoxVerboseLoad.Text = "Verbose load";
            this.checkBoxVerboseLoad.UseVisualStyleBackColor = true;
            // 
            // ProjectSettings
            // 
            this.AcceptButton = this.applyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(600, 379);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Settings";
            this.Load += new System.EventHandler(this.ProjectSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageDescription.ResumeLayout(false);
            this.tabPageDescription.PerformLayout();
            this.tabPageAppearance.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDescription;
        private System.Windows.Forms.TabPage tabPageAppearance;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxProjectTree;
        private System.Windows.Forms.CheckBox checkBoxShowLog;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxVerboseLoad;
    }
}
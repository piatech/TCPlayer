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
using TCPlayer.Project;

namespace TCPlayer.Forms
{
    public partial class ProjectSettings : Form
    {
        private TCProject _project = null;

        public ProjectSettings(TCProject Project)
        {
            InitializeComponent();
            _project = Project;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if(SaveProperties())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private bool SaveProperties()
        {
            _project.SetProperty("Title", textBoxTitle.Text);
            _project.SetProperty("Version", textBoxVersion.Text);
            _project.SetProperty("Author", textBoxAuthor.Text);
            _project.SetProperty("Description", textBoxDescription.Text);
            _project.SetProperty("VerboseLoad", checkBoxVerboseLoad.Checked.ToString());
            _project.SetProperty("ShowProjectTree", checkBoxProjectTree.Checked.ToString());
            _project.SetProperty("ShowLog", checkBoxShowLog.Checked.ToString());

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ProjectSettings_Load(object sender, EventArgs e)
        {
            textBoxTitle.Text = _project.GetProperty("Title");
            textBoxVersion.Text = _project.GetProperty("Version");
            textBoxAuthor.Text = _project.GetProperty("Author");
            textBoxDescription.Text = _project.GetProperty("Description");
            checkBoxVerboseLoad.Checked = _project.GetProperty<bool>("VerboseLoad", false);
            checkBoxProjectTree.Checked = _project.GetProperty<bool>("ShowProjectTree", true);
            checkBoxShowLog.Checked = _project.GetProperty<bool>("ShowLog", true);
        }
    }
}

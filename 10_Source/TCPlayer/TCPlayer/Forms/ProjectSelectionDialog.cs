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
using System.Windows.Forms;

namespace TCPlayer.Forms
{
    public partial class ProjectSelectionDialog : Form
    {
        private MostRecentProjects _mRP;

        public string FileName { get; set; }
        
        public ProjectSelectionDialog()
        {
            InitializeComponent();
            this.Text = TCPlayerMain._applicationName;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = openProjectDialog.FileName;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutTCPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog about = new AboutDialog();
            about.ShowDialog();
        }

        private void ProjectSelectionDialog_Load(object sender, EventArgs e)
        {
            _mRP = new MostRecentProjects(Application.UserAppDataRegistry);

            foreach(Tuple<string, string> value in _mRP)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = 0;
                item.Name = value.Item2;
                item.Text = value.Item1;
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, value.Item2));
                listOfProjects.Items.Add(item);
            }
        }

        private void listOfProjects_DoubleClick(object sender, EventArgs e)
        {
            if(listOfProjects.SelectedItems.Count > 0)
            {
                FileName = listOfProjects.SelectedItems[0].Name;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void clearRecentProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mRP.ClearAll();
            listOfProjects.Items.Clear();
        }
    }
}

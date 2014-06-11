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

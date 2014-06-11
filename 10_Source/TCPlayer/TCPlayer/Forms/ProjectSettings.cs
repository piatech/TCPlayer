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

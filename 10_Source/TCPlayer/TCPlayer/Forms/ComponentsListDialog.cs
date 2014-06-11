using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TCPlayer.Project;

namespace TCPlayer.Forms
{
    public partial class ComponentsListDialog : Form
    {
        public TCProject Project { get; set; }

        public ComponentsListDialog()
        {
            InitializeComponent();
        }

        private void ComponentsListDialog_Load(object sender, EventArgs e)
        {
            foreach(DynComponent component in Project.ComponentSet)
            {
                ListViewItem item = new ListViewItem();

                item.Text = component.ComponentType;

                ListViewItem.ListViewSubItem itemIdent = new ListViewItem.ListViewSubItem();
                itemIdent.Text = component.Ident;
                item.SubItems.Add(itemIdent);

                ListViewItem.ListViewSubItem itemTitle = new ListViewItem.ListViewSubItem();
                itemTitle.Text = component.Title;
                item.SubItems.Add(itemTitle);

                ListViewItem.ListViewSubItem itemVersion = new ListViewItem.ListViewSubItem();
                itemVersion.Text = component.Version.ToString();
                item.SubItems.Add(itemVersion);

                ListViewItem.ListViewSubItem itemAuthor = new ListViewItem.ListViewSubItem();
                itemAuthor.Text = component.Author;
                item.SubItems.Add(itemAuthor);
                
                listOfComponents.Items.Add(item);
            }
        }
    }
}

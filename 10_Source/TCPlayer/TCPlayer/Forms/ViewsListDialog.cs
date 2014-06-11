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
    public partial class ViewsListDialog : Form
    {
        public TCProject Project { get; set; }

        public ViewsListDialog()
        {
            InitializeComponent();
        }

        private void ViewsListDialog_Load(object sender, EventArgs e)
        {
            foreach (DynView view in Project.ViewSet)
            {
                ListViewItem item = new ListViewItem();

                item.Text = view.Name;

                ListViewItem.ListViewSubItem itemIdent = new ListViewItem.ListViewSubItem();
                itemIdent.Text = view.Ident;
                item.SubItems.Add(itemIdent);

                ListViewItem.ListViewSubItem itemTitle = new ListViewItem.ListViewSubItem();
                itemTitle.Text = view.Title;
                item.SubItems.Add(itemTitle);

                ListViewItem.ListViewSubItem itemVersion = new ListViewItem.ListViewSubItem();
                itemVersion.Text = view.Version.ToString();
                item.SubItems.Add(itemVersion);

                ListViewItem.ListViewSubItem itemAuthor = new ListViewItem.ListViewSubItem();
                itemAuthor.Text = view.Author;
                item.SubItems.Add(itemAuthor);

                listOfViews.Items.Add(item);
            }
        }
    }
}

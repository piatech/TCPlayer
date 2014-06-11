using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPlayer.Forms
{
    public partial class AlertBox : Form
    {
        public static IWin32Window OwnerForm { get; set; }
        public static SynchronizationContext UIContext { get; set; }

        public AlertBox()
        {
            InitializeComponent();
            picturePanel.BackgroundImageLayout = ImageLayout.Center;
        }

        public void SetMessage(string Message, string Title, MessageBoxIcon Icon, string Details = null)
        {
            messageOutput.Text = Message;
            Icon icon;

            switch (Icon)
            {
                case MessageBoxIcon.Error:
                    icon = System.Drawing.SystemIcons.Error;
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxIcon.Exclamation:
                    icon = System.Drawing.SystemIcons.Exclamation;
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxIcon.None:
                    icon = null;
                    break;
                default:
                    icon = System.Drawing.SystemIcons.Information;
                    break;
            }

            if (icon != null)
            {
                picturePanel.BackgroundImage = icon.ToBitmap();
            }
            else
            {
                picturePanel.BackgroundImage = null;
            }

            this.Text = Title;

            if (!string.IsNullOrEmpty(Details))
            {
                detailsButton.Visible = true;
                fullMessageOutput.Text = Details;
            }
            else
            {
                detailsButton.Visible = false;
            }

            detailsPanel.Visible = false;
        }

        public static void Show(string Message)
        {
            Show(Message, "", MessageBoxIcon.None);
        }

        public static void Show(string Message, string Title, MessageBoxIcon Icon, string Details = null)
        {
            AlertBox ab = new AlertBox();
            UIContext.Send(s =>
            {
                ab.SetMessage(Message, Title, Icon, Details);
                ab.ShowDialog(OwnerForm);
            }, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AlertBox_Load(object sender, EventArgs e)
        {

        }

        private void detailsButton_Click(object sender, EventArgs e)
        {
            detailsPanel.Visible = !detailsPanel.Visible;
        }

        private void copyLabelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(fullMessageOutput.Text);
        }
    }
}

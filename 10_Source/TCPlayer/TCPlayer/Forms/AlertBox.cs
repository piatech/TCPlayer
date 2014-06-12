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

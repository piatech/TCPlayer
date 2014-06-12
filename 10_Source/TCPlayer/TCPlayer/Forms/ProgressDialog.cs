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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPlayer.API;

namespace TCPlayer.Forms
{
    public partial class ProgressDialog : Form, IProgressEx
    {
        private SynchronizationContext _syncContext;
        private Exception _lastException;
        private bool _cancelable = false;
        private static IWin32Window _ownerForm;

        public bool Canceled { get; set; }
        

        public ProgressDialog(SynchronizationContext UIContext, IWin32Window OwnerForm)
        {
            InitializeComponent();
            _syncContext = UIContext;
            _ownerForm = OwnerForm;
        }

        public void Report(ProgressExValue value)
        {
            _syncContext.Send(s =>
            {
                if (value.Percentage > 0)
                {
                    progressBar.Value = value.Percentage;
                    progressBar.Style = ProgressBarStyle.Continuous;
                }
                else
                {
                    progressBar.Style = ProgressBarStyle.Marquee;
                }

                if (!string.IsNullOrEmpty(value.MainStatus))
                {
                    this.Text = value.MainStatus;
                    statusMessage.Text = value.MainStatus;
                }

                statusDetails.Text = value.SubStatus;
            }, null);
        }

        public void ShowProgress(IProgressExCallback TaskWithProgress)
        {
            _lastException = null;

            Canceled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    TaskWithProgress();
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                }
            }).ContinueWith(t => {
                _syncContext.Send(s =>
                {
                    Hide();
                }, null);
            });

            _syncContext.Send(s =>
            {
                cancelButton.Enabled = _cancelable;
                ControlBox = _cancelable;
                ShowDialog(_ownerForm);
            }, null);

            if (_lastException != null)
            {
                throw _lastException;
            }
        }

        public bool IsCancelable
        {
            get
            {
                return _cancelable;
            }
            set
            {
                _cancelable = value;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            
        }

        private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelButton.Enabled = false;
            this.Text = Resources.Messages.Aborting;
            this.Canceled = true;
            e.Cancel = true;
        }
    }
}
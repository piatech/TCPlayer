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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPlayer.API;

namespace TCPlayer
{
    class StatusBarProgress : IProgressEx
    {
        public ToolStripStatusLabel StatusBarLabel { get; set; }
        public ToolStripProgressBar StatusBarProgressBar { get; set; }
        private SynchronizationContext _uiContext;

        public StatusBarProgress(SynchronizationContext UIContext)
        {
            _uiContext = UIContext;
        }

        public void Report(ProgressExValue value)
        {
            if (value == null)
            {
                Done();
                return;
            }

            if (!String.IsNullOrEmpty(value.MainStatus))
            {
                _uiContext.Send(s =>
                {
                    StatusBarLabel.Text = value.MainStatus;

                    if (!String.IsNullOrEmpty(value.SubStatus))
                    {
                        StatusBarLabel.Text += " (" + value.SubStatus + ")";
                    }
                }, null);
            }

            if (value.Percentage > 0)
            {
                _uiContext.Send(s =>
                {
                    if (StatusBarProgressBar.Style != ProgressBarStyle.Continuous)
                    {
                        StatusBarProgressBar.Style = ProgressBarStyle.Continuous;
                    }

                    StatusBarProgressBar.Visible = true;
                    StatusBarProgressBar.Value = value.Percentage;
                }, null);
            }
            else
            {
                _uiContext.Send(s =>
                {
                    if (StatusBarProgressBar.Style != ProgressBarStyle.Marquee)
                    {
                        StatusBarProgressBar.Style = ProgressBarStyle.Marquee;
                    }
                }, null);
            }
        }

        public void ShowBusyState()
        {
            _uiContext.Send(s =>
            {
                StatusBarProgressBar.Style = ProgressBarStyle.Marquee;
                StatusBarProgressBar.Visible = true;
            }, null);
        }

        public void Done()
        {
            _uiContext.Send(s =>
            {
                StatusBarProgressBar.Visible = false;
                StatusBarLabel.Text = Resources.Messages.Ready;
                StatusBarLabel.ForeColor = Color.Black;
            }, null);
        }

        public void ShowProgress(IProgressExCallback TaskWithProgress)
        {
            throw new NotImplementedException();
        }

        public bool IsCancelable
        {
            get;
            set;
        }

        public bool Canceled { get; set; }
    }
}

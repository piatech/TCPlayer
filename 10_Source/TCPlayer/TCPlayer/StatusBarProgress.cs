// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

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

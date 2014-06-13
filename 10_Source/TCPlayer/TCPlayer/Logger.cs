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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using TCPlayer.Forms;

namespace TCPlayer
{
    public class Logger : ILogger
    {
        public ToolStripStatusLabel StatusBarLabel { get; set; }
        
        
        public ListView LogList
        {
            get
            {
                return _logList;
            }
            set
            {
                _logList = value;
                _logList.DoubleClick += LogList_DoubleClick;
            }
        }

        List<String> _detailsList = new List<string>();
        private ListView _logList;

        public Logger()
        {
            
        }

        void LogList_DoubleClick(object sender, EventArgs e)
        {
            if(LogList.SelectedIndices.Count > 0)
            {
                MessageBox.Show(_detailsList[int.Parse(LogList.SelectedItems[0].Name)]);
            }
        }


        ~Logger()
        {

        }

        public void Log(Exception ex, LogReceiver Receiver = LogReceiver.MessageBox | LogReceiver.Console)
        {
            string details = string.Format("{0}", ex.ToString());

            if (ex is OperationCanceledException)
            {
                Receiver = LogReceiver.Console | LogReceiver.StatusBar;
            }

            Log(ex.Message, 0, LogMessageType.Failure, Receiver, details);
        }

        public void Log(string Message, int MessageId, LogMessageType MessageType = LogMessageType.Information,
            LogReceiver Receiver = LogReceiver.MessageBox | LogReceiver.Console, string Details = null)
        {
            string timeStamp = DateTime.Now.ToString();

            if (Receiver.HasFlag(LogReceiver.Console))
            {
                ListViewItem logItem = new ListViewItem();
                string iconName = null;
                string levelName = null;

                _detailsList.Add(Details);

                switch (MessageType)
                {
                    case LogMessageType.Exclamation:
                        iconName = "exclamation";
                        levelName = Resources.Messages.Exclamation;
                        break;
                    case LogMessageType.Warning:
                        iconName = "warning";
                        levelName = Resources.Messages.Warning;
                        break;
                    case LogMessageType.Success:
                        iconName = "success";
                        levelName = Resources.Messages.Success;
                        break;
                    case LogMessageType.Failure:
                        iconName = "failure";
                        levelName = Resources.Messages.Failure;
                        break;
                    default:
                        iconName = "information";
                        levelName = Resources.Messages.Information;
                        break;
                }

                logItem.Name = (_detailsList.Count - 1).ToString();
                logItem.ImageKey = iconName;
                logItem.Text = levelName;

                logItem.SubItems.Add(Message);
                logItem.SubItems.Add(MessageId != 0 ? MessageId.ToString() : "");
                logItem.SubItems.Add(timeStamp);

                LogList.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        LogList.Items.Add(logItem);
                        
                        if (LogList.Items.Count > 1)
                        {
                            LogList.EnsureVisible(LogList.Items.Count - 1);
                        }
                    }
                    catch(Exception)
                    {
                    }
                }));
            }

            if (Receiver.HasFlag(LogReceiver.StatusBar))
            {
                Color foreColor = Color.Black;

                switch (MessageType)
                {
                    case LogMessageType.Warning:
                        foreColor = Color.Orange;
                        break;
                    case LogMessageType.Failure:
                    case LogMessageType.Exclamation:
                        foreColor = Color.Red;
                        break;
                    default:
                        foreColor = Color.Black;
                        break;
                }

                LogList.BeginInvoke(new Action(() =>
                {
                    StatusBarLabel.Text = Message;
                    StatusBarLabel.ForeColor = foreColor;
                }));

                if (MessageType == LogMessageType.Information || MessageType == LogMessageType.Success)
                {

                }
            }

            if (Receiver.HasFlag(LogReceiver.MessageBox))
            {
                MessageBoxIcon icon;
                string title;

                switch (MessageType)
                {
                    case LogMessageType.Failure:
                        icon = MessageBoxIcon.Error;
                        title = Resources.Messages.Failure;
                        break;
                    case LogMessageType.Warning:
                        icon = MessageBoxIcon.Warning;
                        title = Resources.Messages.Warning;
                        break;
                    default:
                        icon = MessageBoxIcon.Information;
                        title = Resources.Messages.Information;
                        break;
                }

                AlertBox.Show(Message, title, icon, Details);
            }
        }

        public void Ready()
        {
            Log(Resources.Messages.Ready, 0, LogMessageType.Information, LogReceiver.StatusBar);
        }

        public void ClearConsole()
        {
            LogList.BeginInvoke(new Action(() =>
            {
                LogList.Items.Clear();
            }));
        }
    }
}

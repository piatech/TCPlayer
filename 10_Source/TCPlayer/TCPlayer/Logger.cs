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
                        LogList.EnsureVisible(LogList.Items.Count - 1);
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

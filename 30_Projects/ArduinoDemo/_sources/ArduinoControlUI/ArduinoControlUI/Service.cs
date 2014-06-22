using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPlayer.API.Views;

namespace De.Piatech.ArduinoControlUI
{
    public partial class Service : UserControl, IView
    {
        public Service()
        {
            InitializeComponent();
        }

        public string Ident
        {
            get { return "De.Piatech.ArduinoControlUI.Service"; }
        }

        public void LoadPlugin(TCPlayer.API.IProgressEx Progress)
        {
            PluginHost.Icon = Resources.wrench;
        }

        public IViewHost PluginHost
        {
            get;
            set;
        }

        public void UnloadPlugin(TCPlayer.API.IProgressEx Progress)
        {

        }
    }
}

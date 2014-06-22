using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API.Target;

namespace De.Piatech.ArduinoTarget
{
    public class Target : ITarget
    {
        public void Connect(TCPlayer.API.IProgressEx Progress)
        {
            
        }

        public void Disconnect(TCPlayer.API.IProgressEx Progress)
        {
            
        }

        public bool IsConnected()
        {
            return false;
        }

        public string ReadableAddress
        {
            get { return ""; }
        }

        public string Ident
        {
            get { return "De.Piatech.ArduinoTarget"; }
        }

        public void LoadPlugin(TCPlayer.API.IProgressEx Progress)
        {
            
        }

        public TCPlayer.API.Components.IComponentHost PluginHost
        {
            get;
            set;
        }

        public void UnloadPlugin(TCPlayer.API.IProgressEx Progress)
        {

        }
    }
}

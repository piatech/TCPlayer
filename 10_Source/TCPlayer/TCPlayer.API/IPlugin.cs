// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPlayer.API
{
    public interface IPlugin<TPluginHostInterface> where TPluginHostInterface : class
    {
        TPluginHostInterface PluginHost { get; set; }

        String Ident { get; }
        
        // Is called after the plugin is loaded in memory
        void LoadPlugin(IProgressEx Progress);

        // Is called when the project is closing
        void UnloadPlugin(IProgressEx Progress);
    }
}
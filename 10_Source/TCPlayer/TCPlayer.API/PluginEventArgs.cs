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
    public class PluginEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public IProgressEx Progress { get; set; }
    }
}
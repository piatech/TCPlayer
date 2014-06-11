// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPlayer.API.Views
{
    public interface IViewHost : IPluginHost
    {
        Image Icon { get; set; }

        IView Parent { get; }

        bool Online { get; }
        bool Visible { get; }
        
        IViewTimer CreateTimer(bool AutoStart = false);

        event EventHandler<ViewEventArgs> OnViewlessCall;
        event EventHandler<ViewEventArgs> OnBeforeShow;
        event EventHandler<ViewEventArgs> OnShow;
        event EventHandler<ViewEventArgs> OnHide;
        event EventHandler<ViewEventArgs> OnClose;
        event EventHandler<ViewEventArgs> OnRefresh;
    }
}
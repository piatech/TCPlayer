// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.API.Views
{
    public interface IViewTimer
    {
        void Start();
        void Stop();

        event EventHandler Tick;

        double Interval { get; set; }

        bool RunOnlyWhenOnline { get; set; }
        bool RunOnlyWhenVisible { get; set; }

        bool IsRunning { get; }
        bool IsEnabled { get; }
    }
}
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
    public interface IProgressEx
    {
        bool IsCancelable { get; set; }
        bool Canceled { get; set; }
        void ShowProgress(IProgressExCallback TaskWithProgress);
        void Report(ProgressExValue value);
    }

    public delegate void IProgressExCallback();
}
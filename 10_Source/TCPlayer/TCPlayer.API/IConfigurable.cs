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
    public interface IConfigurable
    {
        string GetProperty(string Name, string DefaultValue = "");
        void SetProperty(string Name, string NewValue);
        TType GetProperty<TType>(string Name, TType DefaultValue);
    }
}
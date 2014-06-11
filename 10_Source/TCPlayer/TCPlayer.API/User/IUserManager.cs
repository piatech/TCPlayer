// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API.Components;

namespace TCPlayer.API.User
{
    public interface IUserManager : IComponent
    {
        bool LoggedIn { get; }

    }
}

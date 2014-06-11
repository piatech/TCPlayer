// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API.Components;

namespace TCPlayer.API.Target
{
    public interface ITarget : IComponent
    {
        string ReadableAddress { get; }

        void Connect(IProgressEx Progress);

        void Disconnect(IProgressEx Progress);

        bool IsConnected();
    }
}
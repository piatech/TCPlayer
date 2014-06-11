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
    public enum LogMessageType
    {
        Warning,
        Exclamation,
        Information,
        Failure,
        Success
    }

    [Flags] public enum LogReceiver
    {
        MessageBox = 1,
        Console = 2,
        StatusBar = 4,
        File = 8
    }

    public interface ILogger
    {
        void Log(string Message, int MessageId, LogMessageType MessageType = LogMessageType.Information, 
            LogReceiver Receiver = LogReceiver.MessageBox, string Details = null);
        
        void Log(Exception ex, LogReceiver Receiver = LogReceiver.MessageBox | LogReceiver.Console);

        void Ready();

        void ClearConsole();
    }
}

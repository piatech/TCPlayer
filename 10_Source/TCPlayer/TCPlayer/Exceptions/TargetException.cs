using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class TargetException : Exception
    {
        public TargetException(string Message)
            : base(Message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class ComponentException : Exception
    {
        public ComponentException(string Message)
            : base(Message)
        {
        }
    }
}

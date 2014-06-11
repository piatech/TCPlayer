using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(string Message)
            : base(Message)
        {
        }
    }
}

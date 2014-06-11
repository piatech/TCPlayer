using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string Message)
            : base(Message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class ViewException : Exception
    {
        public ViewException(string Message)
            : base(Message)
        {
        }
    }
}

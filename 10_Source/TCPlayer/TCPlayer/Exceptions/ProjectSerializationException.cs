using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPlayer.Project
{
    public class ProjectSerializationException : Exception
    {
        public ProjectSerializationException(string Message)
            : base(Message)
        {
        }
    }
}

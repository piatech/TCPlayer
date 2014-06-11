using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API;

namespace TCPlayer.Interfaces
{
    interface IPluginSet
    {
        void Load(IProgressEx Progress);
        void Unload(IProgressEx Progress);
        Task<bool> SaveAsync(IProgressEx Progress);
        bool HasExceptions { get; set; }
        bool VerboseLoad { get; set; }
    }
}
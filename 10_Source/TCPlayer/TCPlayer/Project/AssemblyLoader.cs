using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCPlayer.API;

namespace TCPlayer.Project
{
    class AssemblyLoader
    {
        private Dictionary<String, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public TInterface Load<TInterface>(string Path, string Type) where TInterface : class
        {
            Assembly asm;

            // Looking for the assembly in the internal cache
            if (_assemblies.ContainsKey(Path))
            {
                // Assembly found in the cache
                asm = _assemblies[Path];
            }
            else
            {
                // Loading the assembly from file and caching it
                asm = Assembly.LoadFrom(Path);
                _assemblies.Add(Path, asm);
            }

            Type pluginType = null;

            try
            {
                // Searching for the Type in the assembly
                pluginType = asm.GetType(Type);
            }catch(Exception)
            {

            }

            // Type not found
            if (pluginType == null)
            {
                throw new Exception(string.Format(Resources.Messages.PluginTypeNotFoundInAssembly, Type, Path));
            }

            // Creating an instance of the given type
            return (TInterface)asm.CreateInstance(Type);
        }
    }
}

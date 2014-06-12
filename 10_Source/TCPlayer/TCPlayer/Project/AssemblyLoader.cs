#region Copyright (c) 2014 Leonid Lezner. All rights reserved.
// Copyright (C) 2013-2014 Leonid Lezner
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

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

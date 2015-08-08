using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.Modules
{
    public sealed class AssemblyInfo
    {
        public bool IsExecutable { get; set; }

        public Assembly Assembly { get; set; }
    }

	public static class ModuleExtension
	{
        public static IEnumerable<Assembly> LoadModulesAssemblies(string modules)
        {
            return LoadModules(modules).Select(a => a.Assembly).ToList();
        }


		public static IEnumerable<AssemblyInfo> LoadModules(string modules)
		{

			var loc = AppDomain.CurrentDomain.BaseDirectory;
			var loadedModules = modules.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim());

			var result = new List<AssemblyInfo>();
			foreach (string filename in loadedModules)
			{
				try
				{
				    var fileDll = Path.Combine(loc, filename + ".dll");
                    var fileExe = Path.Combine(loc, filename + ".exe");
				    Assembly asm = null;
				    AssemblyInfo asInfo;
				    if (File.Exists(fileDll))
				    {
				        asm = Assembly.Load(File.ReadAllBytes(fileDll));
				        asInfo = new AssemblyInfo()
				            {
				                Assembly = asm,
				                IsExecutable = false
				            };
				    }
				    else
				    {
				        asm = Assembly.Load(File.ReadAllBytes(fileExe));
                        asInfo = new AssemblyInfo()
                        {
                            Assembly = asm,
                            IsExecutable = true
                        };
				    }
				    result.Add(asInfo);
				}
				catch (BadImageFormatException e)
				{
					// Not a valid assembly, move on
				}
			}
			return result;
		}
	}
}
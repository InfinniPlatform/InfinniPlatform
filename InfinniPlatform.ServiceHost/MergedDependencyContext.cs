using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace InfinniPlatform.ServiceHost
{
    public class MergedDependencyContext
    {
        private readonly Dictionary<string, CompilationLibrary> _compilationLibraries;

        private readonly Dictionary<string, RuntimeLibrary> _runtimeLibraries;

        public MergedDependencyContext()
        {
            var defaultDependencyContext = MergeDepsJsonFiles();
            _runtimeLibraries = defaultDependencyContext.RuntimeLibraries.OrderBy(s => s.Name).ToDictionary(s => s.Name, s => s, StringComparer.OrdinalIgnoreCase);
            _compilationLibraries = defaultDependencyContext.CompileLibraries.OrderBy(s => s.Name).ToDictionary(s => s.Name, s => s, StringComparer.OrdinalIgnoreCase);
        }

        private static DependencyContext MergeDepsJsonFiles()
        {
            var defaultDependencyContext = DependencyContext.Default;
            var dependencyContextJsonReader = new DependencyContextJsonReader();
            var depsFiles = Directory.EnumerateFiles(".", "*.deps.json", SearchOption.AllDirectories);

            foreach (var depsFile in depsFiles)
            {
                using (var stream = new FileStream(depsFile, FileMode.Open))
                {
                    var otherDependencyContext = dependencyContextJsonReader.Read(stream);
                    defaultDependencyContext = defaultDependencyContext.Merge(otherDependencyContext);
                }
            }
            return defaultDependencyContext;
        }

        public RuntimeLibrary GetRuntimeLibrary(AssemblyName assemblyName)
        {
            try
            {
                return _runtimeLibraries[assemblyName.Name];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
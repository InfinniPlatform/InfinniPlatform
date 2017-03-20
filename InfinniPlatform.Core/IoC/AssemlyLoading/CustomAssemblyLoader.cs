using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Hosting;

namespace InfinniPlatform.Core.IoC.AssemlyLoading
{
    public class CustomAssemblyLoader : AssemblyLoadContext
    {
        private static readonly MergedDependencyContext MergedDependencyContext = new MergedDependencyContext();
        private static readonly Lazy<Dictionary<string, string>> LocalAssembliesCache = new Lazy<Dictionary<string, string>>(BuildLocalAssembliesCache);

        private static string NugetProbingPath => Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".nuget", "packages");

        /// <summary>
        /// Инициализирует дополнительную логику загрузки сборок.
        /// </summary>
        public static void UseCustomAssemblyLoading(IApplicationLifetime appLifetime)
        {
            //appLifetime.ApplicationStarted.Register(() => AssemblyLoadContext.Default.Resolving += DefaultOnResolving);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (LocalAssembliesCache.Value.ContainsKey(assemblyName.Name))
            {
                var localAssemblyPath = LocalAssembliesCache.Value[assemblyName.Name];
                return LoadFromAssemblyPath(localAssemblyPath);
            }

            var runtimeLibrary = MergedDependencyContext.GetRuntimeLibrary(assemblyName);

            var nugetPackagePath = runtimeLibrary?.Path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var nugetAssemblyPath = runtimeLibrary?.RuntimeAssemblyGroups.FirstOrDefault()
                                                  ?.AssetPaths.FirstOrDefault()
                                                  ?.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var fullAssemblyPath = Path.Combine(NugetProbingPath, nugetPackagePath, nugetAssemblyPath ?? string.Empty);

            if (File.Exists(fullAssemblyPath))
            {
                return LoadFromAssemblyPath(fullAssemblyPath);
            }
            if (Directory.Exists(fullAssemblyPath))
            {
                var assemblyPathInLib = Directory.EnumerateFiles(Path.Combine(fullAssemblyPath, "lib"), "*.dll", SearchOption.AllDirectories)
                                                 .FirstOrDefault(path => path.Contains("netstandard"));

                if (assemblyPathInLib != null)
                {
                    return LoadFromAssemblyPath(assemblyPathInLib);
                }

                var assemblyPathInRuntimes = Directory.EnumerateFiles(Path.Combine(fullAssemblyPath, "runtimes"), "*.dll", SearchOption.AllDirectories)
                                                      .FirstOrDefault(path => path.Contains("netstandard") && path.Contains("win"));

                if (assemblyPathInRuntimes != null)
                {
                    return LoadFromAssemblyPath(assemblyPathInRuntimes);
                }
            }

            return null;
        }

        private static Dictionary<string, string> BuildLocalAssembliesCache()
        {
            var localAssemblies = new Dictionary<string, string>();
            var path = Directory.GetFiles(".", "*.dll", SearchOption.AllDirectories);

            foreach (var s in path)
            {
                localAssemblies[Path.GetFileNameWithoutExtension(s)] = Path.GetFullPath(s);
            }

            return localAssemblies;
        }
    }
}
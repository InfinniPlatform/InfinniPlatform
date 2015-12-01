using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
    public sealed class AssemblyDiscovery
    {
        public IList<SourceAssemblyInfo> SourceAssemblyList { get; } = new List<SourceAssemblyInfo>();

        public bool DiscoverAppliedAssemblies(string configurationId)
        {
            var assemblies = PackageMetadataLoader.GetAssemblies(configurationId)
                                                  .ToArray();

            if (assemblies.Any())
            {
                var assembliesNames = assemblies.Select(ass => (string)ass.Name).ToArray();
                LoadSourceAssembliesForConfig(assembliesNames);
            }
            else
            {
                LoadSourceAssembliesForConfig();
            }

            return SourceAssemblyList.Any();
        }

        private void LoadSourceAssembliesForConfig()
        {
            SourceAssemblyList.Clear();

            var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
            var asssemblyFiles = Directory.GetFiles(assemblyPath, "*.dll");

            foreach (var asssemblyFile in asssemblyFiles)
            {
                var asssemblyFilePath = Path.GetFullPath(asssemblyFile);

                var assembly = Assembly.LoadFile(asssemblyFilePath);

                SourceAssemblyList.Add(new SourceAssemblyInfo
                                       {
                                           Name = Path.GetFileNameWithoutExtension(asssemblyFile),
                                           AssemblyFileName = asssemblyFilePath,
                                           Assembly = assembly
                                       });
            }
        }

        private void LoadSourceAssembliesForConfig(IEnumerable<string> assemblyNames)
        {
            SourceAssemblyList.Clear();

            var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var assemblyName in assemblyNames)
            {
                var asssemblyFilePath = Path.GetFullPath(Path.Combine(assemblyPath, assemblyName + ".dll"));

                var assembly = Assembly.LoadFile(asssemblyFilePath);

                SourceAssemblyList.Add(new SourceAssemblyInfo
                                       {
                                           Name = Path.GetFileNameWithoutExtension(asssemblyFilePath),
                                           AssemblyFileName = asssemblyFilePath,
                                           Assembly = assembly
                                       });
            }
        }
    }
}
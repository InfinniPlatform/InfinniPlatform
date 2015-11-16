using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
    public sealed class AssemblyDiscovery
    {
        public IList<SourceAssemblyInfo> SourceAssemblyList { get; } = new List<SourceAssemblyInfo>();

        public bool DiscoverAppliedAssemblies(string configurationId)
        {
            LoadSourceAssembliesForConfig();

            return (SourceAssemblyList.Count > 0);
        }

        private void LoadSourceAssembliesForConfig()
        {
            SourceAssemblyList.Clear();

            var assemblyPath = AppSettings.GetValue("AppliedAssemblies", Path.Combine("..", "Assemblies"));
            var asssemblyFiles = Directory.GetFiles(assemblyPath, "*.dll");

            foreach (var asssemblyFile in asssemblyFiles)
            {
                try
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
                catch
                {
                }
            }
        }
    }
}
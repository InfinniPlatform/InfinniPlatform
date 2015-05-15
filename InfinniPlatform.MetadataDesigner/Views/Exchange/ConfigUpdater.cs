using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
	public sealed class ConfigUpdater
	{
		private readonly string _versionName;
		private readonly AssemblyDiscovery _assemblyDiscovery;

		public ConfigUpdater(string versionName)
		{
			_versionName = versionName;
			_assemblyDiscovery = new AssemblyDiscovery();
		}


		public IList<SourceAssemblyInfo> SourceAssemblyList
		{
			get { return _assemblyDiscovery.SourceAssemblyList; }
		}

		public string VersionName
		{
			get { return _versionName; }
		}

		public void UpdateConfigurationMetadataFromZip(string fileName)
		{
            UpdateApi.UpdateConfigFromJson(VersionName, fileName);
		}

	    public void UpdateConfigurationMetadataFromDirectory(string pathToConfigDirectory)
	    {
	        new ConfigExporter(new DirectoryStructure(pathToConfigDirectory)).ImportHeaderFromStructure(VersionName);
	    }

	    public void UpdateConfigurationAppliedAssemblies(string configuration)
        {
            try
            {
                if (!_assemblyDiscovery.DiscoverAppliedAssemblies(configuration))
                {
                    var failedAssemblies = string.Join(",",
                        _assemblyDiscovery.SourceAssemblyList.Where(s => s.Assembly == null).Select(s => s.Name));
                    throw new ArgumentException(string.Format(
                        "Не найдены прикладные сборки для обновления конфигурации. Сборки: {0}", failedAssemblies));
                }

                var instance = new ConfigurationInfo();
                var appliedAssemblies = new List<string>();
                instance.AppliedAssemblyList = appliedAssemblies;

                appliedAssemblies.AddRange(SourceAssemblyList.Select(updatePackageAssembly => updatePackageAssembly.AssemblyFileName));

                var packageBuilder = new PackageBuilder();
                foreach (var appliedAssembly in instance.AppliedAssemblyList)
                {
                    var package = packageBuilder.BuildPackage(configuration, VersionName, appliedAssembly);
                    UpdateApi.InstallPackages(new[] { package });
                    Console.WriteLine(@"Assembly ""{0}"" installed", appliedAssembly);
                }

                RestQueryApi.QueryPostNotify(configuration);
                UpdateApi.UpdateStore(configuration);
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Error during assemblies update: {0}", e.Message));
            }
        }
	}
}
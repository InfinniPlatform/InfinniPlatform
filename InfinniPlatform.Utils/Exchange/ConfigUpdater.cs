using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Utils.Exchange
{
	public sealed class ConfigUpdater
	{
		public ConfigUpdater(string versionName)
		{
			_versionName = versionName;
			_assemblyDiscovery = new AssemblyDiscovery();
		}

		readonly AssemblyDiscovery _assemblyDiscovery;
		readonly string _versionName;

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
			new UpdateApi().UpdateConfigFromJson(fileName);
		}

		public dynamic UpdateSolutionMetadataFromZip(string fileName)
		{
			return new UpdateApi().UpdateSolutionFromJson(fileName);
		}

		public void UpdateConfigurationMetadataFromDirectory(string pathToConfigDirectory)
		{
			new ConfigExporter(new DirectoryStructure(pathToConfigDirectory)).ImportHeaderFromStructure(VersionName);
		}

		public void UpdateConfigurationAppliedAssemblies(string configuration)
		{
			try
			{
				if (!_assemblyDiscovery.DiscoverAppliedAssemblies(VersionName, configuration))
				{
					var failedAssemblies = string.Join(",",
													   _assemblyDiscovery.SourceAssemblyList.Where(s => s.Assembly == null).Select(s => s.Name));
					throw new ArgumentException(string.Format(
															  "Не найдены прикладные сборки для обновления конфигурации. Сборки: {0}", failedAssemblies));
				}

				var appliedAssemblies = SourceAssemblyList.Select(updatePackageAssembly => updatePackageAssembly.AssemblyFileName);

				var packageBuilder = new PackageBuilder();
				foreach (var appliedAssembly in appliedAssemblies)
				{
					var package = packageBuilder.BuildPackage(configuration, VersionName, appliedAssembly);
					new UpdateApi().InstallPackages(new[] { package });
					Console.WriteLine(@"Assembly ""{0}"" installed", appliedAssembly);
				}

				RestQueryApi.QueryPostNotify(configuration);
				new UpdateApi().UpdateStore(configuration);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error during assemblies update: {0}", e.Message);
			}
		}
	}
}
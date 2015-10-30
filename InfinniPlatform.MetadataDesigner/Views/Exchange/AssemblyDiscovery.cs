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
		private readonly IList<SourceAssemblyInfo> _sourceAssemblyList = new List<SourceAssemblyInfo>();

		public IList<SourceAssemblyInfo> SourceAssemblyList
		{
			get { return _sourceAssemblyList; }
		}

		public bool DiscoverAppliedAssemblies(string configurationId)
		{
			LoadSourceAssembliesForConfig(configurationId);

			foreach (var sourceAssemblyInfo in SourceAssemblyList)
			{
				var relativePath = AppSettings.GetValue("AppliedAssemblies", "..\\Assemblies");
				var pathToAssemblies = relativePath != null
										   ? Path.GetFullPath(relativePath)
										   : Directory.GetCurrentDirectory();

				if (sourceAssemblyInfo.Assembly == null)
				{
					var assemblyFileName = Path.Combine(pathToAssemblies, string.Concat(sourceAssemblyInfo.Name, ".dll"));
					var executableFileName = Path.Combine(pathToAssemblies, string.Concat(sourceAssemblyInfo.Name, ".exe"));

					if (File.Exists(assemblyFileName))
					{
						sourceAssemblyInfo.Assembly = Assembly.Load(File.ReadAllBytes(assemblyFileName));
						sourceAssemblyInfo.AssemblyFileName = assemblyFileName;
					}
					else if (File.Exists(executableFileName))
					{
						sourceAssemblyInfo.Assembly = Assembly.Load(File.ReadAllBytes(executableFileName));
						sourceAssemblyInfo.AssemblyFileName = executableFileName;
					}
				}
			}

			return SourceAssemblyList.All(a => a.Assembly != null);
		}

		private void LoadSourceAssembliesForConfig(string configurationId)
		{
			SourceAssemblyList.Clear();

			dynamic configs = PackageMetadataLoader.Configurations[configurationId];

			var items = configs.Content.Assemblies;
			foreach (var item in items)
			{
				SourceAssemblyList.Add(new SourceAssemblyInfo
									   {
										   Name = item.Name
									   });
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Settings;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
	public sealed class AssemblyDiscovery
	{
		private readonly IList<SourceAssemblyInfo> _sourceAssemblyList = new List<SourceAssemblyInfo>();
        
		public bool DiscoverAppliedAssemblies(string version, string configurationId)
		{
			LoadSourceAssembliesForConfig(version, configurationId);

		    var folderExtension = DateTime.Now.ToString("yyyyMMddHHmmss");
			foreach (var sourceAssemblyInfo in SourceAssemblyList)
			{
				var relativePath = AppSettings.GetValue("AppliedAssemblies");
				var pathToAssemblies = relativePath != null ? Path.GetFullPath(relativePath) : Directory.GetCurrentDirectory();

				if (sourceAssemblyInfo.Assembly == null)
				{
					var assemblyFileName = Path.Combine(pathToAssemblies, sourceAssemblyInfo.Name);
					var copyDir = Path.Combine(Path.GetDirectoryName(assemblyFileName), "temp_" + folderExtension);
                    if (!Directory.Exists(copyDir))
                    {
                        Directory.CreateDirectory(copyDir);
                    }

					if (File.Exists(assemblyFileName + ".dll"))
					{
						var copyFileName = Path.Combine(copyDir, sourceAssemblyInfo.Name + ".dll");
                        File.Copy(assemblyFileName + ".dll",copyFileName,true);

					    sourceAssemblyInfo.Assembly = Assembly.Load(File.ReadAllBytes(copyFileName));
					    sourceAssemblyInfo.AssemblyFileName = assemblyFileName + ".dll";
					    
					}
					else if (File.Exists(assemblyFileName + ".exe"))
					{
						var copyFileName = Path.Combine(copyDir, sourceAssemblyInfo.Name + ".exe");
						File.Copy(assemblyFileName + ".exe", copyFileName,true);

					    sourceAssemblyInfo.Assembly = Assembly.Load(File.ReadAllBytes(assemblyFileName + ".exe"));
                        sourceAssemblyInfo.AssemblyFileName = assemblyFileName + ".exe";
					}
				}
			}
			if (SourceAssemblyList.Any(a => a.Assembly == null))
			{
				return false;
			}
			return true;
		}

	    private void LoadSourceAssembliesForConfig(string version, string configurationId)
		{
			SourceAssemblyList.Clear();

			var reader = new ManagerFactoryConfiguration(version, configurationId).BuildAssemblyMetadataReader();
			var items = reader.GetItems();
			foreach (var item in items)
			{
				SourceAssemblyList.Add(new SourceAssemblyInfo()
				{
					Name = item.Name
				});
			}
		}

		public IList<SourceAssemblyInfo> SourceAssemblyList
		{
			get { return _sourceAssemblyList; }
		}
	}
}

using System;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.Utils.Update;

namespace InfinniPlatform.Utils.Exchange
{
	public sealed class ExchangeDirectorSolution
	{
		public ExchangeDirectorSolution(IUpdatePrepareConfig updatePrepareConfig, string solutionId)
		{
			_updatePrepareConfig = updatePrepareConfig;
			_solutionId = solutionId;

			_solutionArchiveName = string.Format("exportsolution_{0}.zip", _solutionId);
		}

		readonly string _solutionArchiveName;
		readonly string _solutionId;
		readonly IUpdatePrepareConfig _updatePrepareConfig;

		public void ExportJsonSolutionToDirectory(string exportDir, string version, string newVersion)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				new SolutionExporter(new DirectoryStructure(exportDir),
									 config => new DirectoryStructure(exportDir + string.Format(@"\{0}_{1}", config.Name, newVersion))).ExportSolutionToStructure(_solutionId, version, newVersion);
			}
		}

		public dynamic UpdateSolutionMetadataFromDirectory(string importDir)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(GetVersionFromPath(importDir));
				ZipLoader.CreateZipArchiveFromDirectory(importDir, _solutionArchiveName);
				return configUpdater.UpdateSolutionMetadataFromZip(_solutionArchiveName);
			}
			return null;
		}

		public void UpdateConfigurationAppliedAssemblies(dynamic solution)
		{
			foreach (var configuration in solution.ReferencedConfigurations)
			{
				if (_updatePrepareConfig.PrepareRoutingOperation())
				{
					var configUpdater = new ConfigUpdater(configuration.Version.ToString());
					configUpdater.UpdateConfigurationAppliedAssemblies(configuration.Name.ToString());
				}
			}
		}

		string GetVersionFromPath(string path)
		{
			var dir = Path.GetFileName(path);
			var pathConfig = dir.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
			if (pathConfig.Count() == 2)
			{
				return pathConfig[1];
			}
			return null;
		}
	}
}
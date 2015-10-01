using System;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.MetadataDesigner.Views.Update;
using System.IO;
using System.Linq;
using DevExpress.Data.PLinq.Helpers;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
	public sealed class ExchangeDirector
	{
		private readonly IUpdatePrepareConfig _updatePrepareConfig;
		private readonly string _configurationId;

        private readonly string _configurationArchiveName;

	    public ExchangeDirector(IUpdatePrepareConfig updatePrepareConfig, string configurationId)
		{
			_updatePrepareConfig = updatePrepareConfig;
			_configurationId = configurationId;

		    _configurationArchiveName = string.Format("exportconfig_{0}.zip", _configurationId);
		}

		public void ExportJsonConfigToZip(string fileName, string version, string newVersion)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				new ConfigExporter(new ZipStructure(fileName)).ExportHeaderToStructure(version, _configurationId, newVersion);
			}
		}

		public void ExportJsonConfigToDirectory(string exportDir, string version, string newVersion)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				new ConfigExporter(new DirectoryStructure(exportDir)).ExportHeaderToStructure(version, _configurationId, newVersion);
			}
		}

		public void UpdateConfigurationMetadataFromDirectory(string pathToDirectory)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(GetVersionFromPath(pathToDirectory));
                ZipLoader.CreateZipArchiveFromDirectory(pathToDirectory, _configurationArchiveName);
                configUpdater.UpdateConfigurationMetadataFromZip(_configurationArchiveName);
			}
		}

	    private string GetVersionFromPath(string path)
	    {
	        var dir = Path.GetFileName(path);
            var pathConfig = dir.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (pathConfig.Count() == 2)
            {
                return pathConfig[1];
            }
            return null; 
	    }

	    public void UpdateConfigurationMetadataFromZip(string fileName)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(GetVersionFromPath(fileName));
				configUpdater.UpdateConfigurationMetadataFromZip(fileName);
			}
		}

	    public void UpdateConfigurationAppliedAssemblies()
        {
            if (_updatePrepareConfig.PrepareRoutingOperation())
            {
                var configUpdater = new ConfigUpdater(_updatePrepareConfig.Version);
                configUpdater.UpdateConfigurationAppliedAssemblies(_configurationId);
            }
        }


	}
}
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Packages.ConfigStructure;
using InfinniPlatform.MetadataDesigner.Views.Update;
using System.IO;

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

		public void ExportJsonConfigToZip(string fileName)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				new ConfigExporter(new ZipStructure(fileName)).ExportHeaderToStructure(_configurationId);
			}
		}

		public void ExportJsonConfigToDirectory(string exportDir)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				new ConfigExporter(new DirectoryStructure(exportDir)).ExportHeaderToStructure(_configurationId);
			}
		}

		public void UpdateConfigurationMetadataFromSelf()
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(_updatePrepareConfig.Version);
                ExportJsonConfigToZip(Path.Combine(Directory.GetCurrentDirectory(), _configurationArchiveName));
                configUpdater.UpdateConfigurationMetadataFromZip(_configurationArchiveName);
			}
		}

		public void UpdateConfigurationMetadataFromDirectory(string pathToDirectory)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(_updatePrepareConfig.Version);
                ZipLoader.CreateZipArchiveFromDirectory(pathToDirectory, _configurationArchiveName);
                configUpdater.UpdateConfigurationMetadataFromZip(_configurationArchiveName);
			}
		}

		public void UpdateConfigurationMetadataFromZip(string fileName)
		{
			if (_updatePrepareConfig.PrepareRoutingOperation())
			{
				var configUpdater = new ConfigUpdater(_updatePrepareConfig.Version);
				configUpdater.UpdateConfigurationMetadataFromZip(fileName);
			}
		}

        public void UpdateConfigurationAppliedAssemblies(string configuration)
        {
            if (_updatePrepareConfig.PrepareRoutingOperation())
            {
                var configUpdater = new ConfigUpdater(_updatePrepareConfig.Version);
                configUpdater.UpdateConfigurationAppliedAssemblies(configuration);
            }
        }
	}
}
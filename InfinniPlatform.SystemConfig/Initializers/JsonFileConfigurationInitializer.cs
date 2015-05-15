using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.File;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.SystemConfig.Initializers
{
    public sealed class JsonFileConfigurationInitializer : IStartupInitializer
    {
        private readonly string _fileConfigDir;

		private readonly IChangeListener _changeListener;
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly JsonFileConfigManager _manager;

        public JsonFileConfigurationInitializer(IChangeListener changeListener, IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            var appSettingsPath = AppSettings.GetValue("ConfigurationPath");
            _fileConfigDir = appSettingsPath != null
                                 ? Path.GetFullPath(appSettingsPath)
                                 : Directory.GetCurrentDirectory();

			_changeListener = changeListener;
			_metadataConfigurationProvider = metadataConfigurationProvider;
            _manager = new JsonFileConfigManager(_fileConfigDir);
		}

        public void OnStart(HostingContextBuilder contextBuilder)
        {
            
            IEnumerable<dynamic> configurations = _manager.GetConfigurationList();

            _changeListener.RegisterOnChange("JsonFileConfig", OnChangeModules);
            foreach (var configuration in configurations)
            {
                InstallConfiguration(configuration);
            }	
        }

        private void InstallConfiguration(string configurationId)
        {
            IMetadataConfiguration metadataConfig =
                InfinniPlatformHostServer.Instance.CreateConfiguration(configurationId, false);
            var installer = new JsonConfigurationInstaller(new JsonConfigReaderFile(_manager));

            installer.InstallConfiguration(metadataConfig);
            metadataConfig.ScriptConfiguration.InitActionUnitStorage();

            InfinniPlatformHostServer.Instance.InstallServices(metadataConfig.ServiceRegistrationContainer);
        }

        /// <summary>
        ///   Обновление конфигурации при получении события обновления сборок
        ///   Пока атомарность обновления не обеспечивается - в момент обновления обращающиеся к серверу запросы получат отлуп
        /// </summary>
        /// <param name="configurationId">Идентификатор конфигурации</param>
        private void OnChangeModules(string configurationId)
        {
            //если конфигурация уже установлена - перезагружаем метаданные конфигурации и прикладные сборки
            var config = _metadataConfigurationProvider.GetMetadataConfiguration(configurationId);

            //если обновляется несистемная конфигурация (не встроенная конфигурация), то чистим метаданные загруженной конфигурации
            if (config != null && !config.IsEmbeddedConfiguration)
            {
                InfinniPlatformHostServer.Instance.RemoveConfiguration(configurationId);
                InfinniPlatformHostServer.Instance.UninstallServices(configurationId);
            }

            //если конфигурация еще не загружена или это не системная (встроенная) конфигурация, то устанавливаем конфигурацию
            if (config == null || !config.IsEmbeddedConfiguration)
            {
                InstallConfiguration(configurationId);
            }

        }
    }
}

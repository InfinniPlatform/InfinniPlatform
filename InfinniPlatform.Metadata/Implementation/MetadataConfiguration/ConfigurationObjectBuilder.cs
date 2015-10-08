using System;
using System.Collections.Generic;
using System.Diagnostics;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///     Конструктор объекта конфигурации предметной области.
    ///     Предоставляет функционал для создания объектов конфигурации,
    ///     используемых на уровне прикладных скриптов
    /// </summary>
    public sealed class ConfigurationObjectBuilder : IConfigurationObjectBuilder
    {
        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly IIndexFactory _indexFactory;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
	    private readonly IScriptConfiguration _scriptConfiguration;

	    public ConfigurationObjectBuilder(IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory,
            IMetadataConfigurationProvider metadataConfigurationProvider, IScriptConfiguration scriptConfiguration)
        {
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _metadataConfigurationProvider = metadataConfigurationProvider;
		    _scriptConfiguration = scriptConfiguration;
        }

        /// <summary>
        ///     Получить объект конфигурации метаданных для указанного идентификатора
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="metadataIdentifier">Идентификатор метаданных</param>
        /// <returns>Объект конфигурации метаданных</returns>
        public IConfigurationObject GetConfigurationObject(string version, string metadataIdentifier)
        {
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(version, metadataIdentifier);
            if (metadataConfiguration == null)
            {
				// Для тестов %) т.к. теперь метаданные загружаются только с диска
				metadataConfiguration = _metadataConfigurationProvider.AddConfiguration(version, metadataIdentifier, _scriptConfiguration, false);
                // Logger.Log.Error(string.Format("Metadata configuration not registered: \"{0}\"", metadataIdentifier));
                // return null;
            }
            return new ConfigurationObject(metadataConfiguration, _indexFactory, _blobStorageFactory);
        }

        /// <summary>
        ///     Получить список зарегистрированных конфигураций
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMetadataConfiguration> GetConfigurationList()
        {
            return _metadataConfigurationProvider.Configurations;
        }

        public IEnumerable<Tuple<string, string>> GetConfigurationVersions()
        {
            return _metadataConfigurationProvider.ConfigurationVersions;
        }
    }
}
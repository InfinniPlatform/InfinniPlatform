using System.Collections.Generic;

using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    /// Конструктор объекта конфигурации предметной области.
    /// Предоставляет функционал для создания объектов конфигурации,
    /// используемых на уровне прикладных скриптов
    /// </summary>
    public sealed class ConfigurationObjectBuilder : IConfigurationObjectBuilder
    {
        public ConfigurationObjectBuilder(IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory, IMetadataConfigurationProvider metadataConfigurationProvider, IScriptConfiguration scriptConfiguration)
        {
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _metadataConfigurationProvider = metadataConfigurationProvider;
            _scriptConfiguration = scriptConfiguration;
        }

        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly IIndexFactory _indexFactory;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly IScriptConfiguration _scriptConfiguration;

        /// <summary>
        /// Получить объект конфигурации метаданных для указанного идентификатора
        /// </summary>
        /// <param name="metadataIdentifier">Идентификатор метаданных</param>
        /// <returns>Объект конфигурации метаданных</returns>
        public IConfigurationObject GetConfigurationObject(string metadataIdentifier)
        {
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(metadataIdentifier);
            if (metadataConfiguration == null)
            {
                // Для тестов %) т.к. теперь метаданные загружаются только с диска
                metadataConfiguration = _metadataConfigurationProvider.AddConfiguration(metadataIdentifier, _scriptConfiguration, false);
                // Logger.Log.Error(string.Format("Metadata configuration not registered: \"{0}\"", metadataIdentifier));
                // return null;
            }
            return new ConfigurationObject(metadataConfiguration, _indexFactory, _blobStorageFactory);
        }

        /// <summary>
        /// Получить список зарегистрированных конфигураций
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IMetadataConfiguration> GetConfigurationList()
        {
            return _metadataConfigurationProvider.Configurations;
        }
    }
}
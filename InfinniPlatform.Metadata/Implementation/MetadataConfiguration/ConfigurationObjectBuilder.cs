using System.Collections.Generic;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;

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

        public ConfigurationObjectBuilder(IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory,
            IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }

        /// <summary>
        ///     Получить объект конфигурации метаданных для указанного идентификатора
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="metadataIdentifier">Идентификатор метаданных</param>
        /// <returns>Объект конфигурации метаданных</returns>
        public IConfigurationObject GetConfigurationObject(string version, string metadataIdentifier)
        {
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(version,
                metadataIdentifier);
            if (metadataConfiguration == null)
            {
                Logger.Log.Error(string.Format("Metadata configuration not registered: \"{0}\"",
                    metadataIdentifier));
                return null;
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
    }
}
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Factories;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Конструктор объекта конфигурации предметной области.
    /// Предоставляет функционал для создания объектов конфигурации,
    /// используемых на уровне прикладных скриптов
    /// </summary>
    public sealed class ConfigurationObjectBuilder : IConfigurationObjectBuilder
    {
        public ConfigurationObjectBuilder(IIndexFactory indexFactory, IConfigurationMetadataProvider configurationMetadataProvider)
        {
            _indexFactory = indexFactory;
            _configurationMetadataProvider = configurationMetadataProvider;
        }

        private readonly IIndexFactory _indexFactory;
        private readonly IConfigurationMetadataProvider _configurationMetadataProvider;

        /// <summary>
        /// Получить объект конфигурации метаданных для указанного идентификатора
        /// </summary>
        public IConfigurationObject GetConfigurationObject(string configuration)
        {
            var metadataConfiguration = _configurationMetadataProvider.GetConfiguration(configuration);

            return new ConfigurationObject(metadataConfiguration, _indexFactory);
        }

        /// <summary>
        /// Получить список зарегистрированных конфигураций
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationMetadata> GetConfigurationList()
        {
            var configurationNames = _configurationMetadataProvider.GetConfigurationNames();

            return configurationNames.Select(i => _configurationMetadataProvider.GetConfiguration(i));
        }
    }
}
using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Конструктор объекта конфигурации предметной области.
    /// Предоставляет функционал для создания объектов конфигурации,
    /// используемых на уровне прикладных скриптов
    /// </summary>
    public sealed class ConfigurationObjectBuilder : IConfigurationObjectBuilder
    {
        public ConfigurationObjectBuilder(IIndexFactory indexFactory, IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _indexFactory = indexFactory;
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }

        private readonly IIndexFactory _indexFactory;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

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
                metadataConfiguration = _metadataConfigurationProvider.AddConfiguration(metadataIdentifier, false);
                // Logger.Log.Error(string.Format("Metadata configuration not registered: \"{0}\"", metadataIdentifier));
                // return null;
            }
            return new ConfigurationObject(metadataConfiguration, _indexFactory);
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
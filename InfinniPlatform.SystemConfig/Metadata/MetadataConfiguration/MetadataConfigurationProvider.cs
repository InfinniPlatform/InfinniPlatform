using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.SystemConfig.Metadata.MetadataConfiguration
{
    /// <summary>
    /// Провайдер для работы с конфигурациями метаданных
    /// </summary>
    internal sealed class MetadataConfigurationProvider : IMetadataConfigurationProvider
    {
        public MetadataConfigurationProvider(IServiceRegistrationContainerFactory serviceRegistrationContainerFactory, IServiceTemplateConfiguration serviceTemplateConfiguration, IScriptConfiguration scriptConfiguration)
        {
            _serviceRegistrationContainerFactory = serviceRegistrationContainerFactory;
            _serviceTemplateConfiguration = serviceTemplateConfiguration;
            _scriptConfiguration = scriptConfiguration;

            _configurations = new ConcurrentDictionary<string, IMetadataConfiguration>(StringComparer.OrdinalIgnoreCase);
        }


        private readonly IServiceRegistrationContainerFactory _serviceRegistrationContainerFactory;
        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;
        private readonly IScriptConfiguration _scriptConfiguration;

        private readonly ConcurrentDictionary<string, IMetadataConfiguration> _configurations;


        public IEnumerable<IMetadataConfiguration> Configurations => _configurations.Values;


        /// <summary>
        /// Удалить указанную конфигурацию метаданных из списка загруженных конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        public void RemoveConfiguration(string metadataConfigurationId)
        {
            IMetadataConfiguration metadataConfiguration;

            _configurations.TryRemove(metadataConfigurationId, out metadataConfiguration);
        }

        /// <summary>
        /// Получить метаданные конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId)
        {
            IMetadataConfiguration metadataConfiguration;

            _configurations.TryGetValue(metadataConfigurationId, out metadataConfiguration);

            return metadataConfiguration;
        }

        /// <summary>
        /// Добавить конфигурацию метаданных
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
        /// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
        /// <returns>Конфигурация метаданных</returns>
        public IMetadataConfiguration AddConfiguration(string metadataConfigurationId, bool isEmbeddedConfiguration)
        {
            IMetadataConfiguration metadataConfiguration;

            if (!_configurations.TryGetValue(metadataConfigurationId, out metadataConfiguration))
            {
                var buildServiceRegistrationContainer = _serviceRegistrationContainerFactory.BuildServiceRegistrationContainer(metadataConfigurationId);

                metadataConfiguration = new MetadataConfiguration(_scriptConfiguration, buildServiceRegistrationContainer, _serviceTemplateConfiguration, isEmbeddedConfiguration)
                {
                    ConfigurationId = metadataConfigurationId,
                };

                _configurations.TryAdd(metadataConfigurationId, metadataConfiguration);
            }

            return metadataConfiguration;
        }
    }
}
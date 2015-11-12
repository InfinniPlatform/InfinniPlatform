using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    /// Провайдер для работы с конфигурациями метаданных
    /// </summary>
    public class MetadataConfigurationProvider : IMetadataConfigurationProvider
    {
        private const string CacheConfigListKey = "___configurations";

        public MetadataConfigurationProvider(IServiceRegistrationContainerFactory serviceRegistrationContainerFactory, IServiceTemplateConfiguration serviceTemplateConfiguration, ISharedCacheComponent sharedCacheComponent)
        {
            _serviceRegistrationContainerFactory = serviceRegistrationContainerFactory;
            _serviceTemplateConfiguration = serviceTemplateConfiguration;
            _sharedCacheComponent = sharedCacheComponent;

            lock (_configLock)
            {
                sharedCacheComponent.Lock();
                try
                {
                    var configurations = sharedCacheComponent.Get(CacheConfigListKey);
                    if (configurations == null)
                    {
                        sharedCacheComponent.Set(CacheConfigListKey, _configurationList);
                    }
                }
                finally
                {
                    sharedCacheComponent.Unlock();
                }
            }
        }

        private readonly object _configLock = new object();
        private readonly IServiceRegistrationContainerFactory _serviceRegistrationContainerFactory;
        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;
        private readonly ISharedCacheComponent _sharedCacheComponent;
        private volatile List<IMetadataConfiguration> _configurationList = new List<IMetadataConfiguration>();

        /// <summary>
        /// Список конфигураций метаданных
        /// </summary>
        public IEnumerable<IMetadataConfiguration> Configurations
        {
            get { return (List<IMetadataConfiguration>)_sharedCacheComponent.Get(CacheConfigListKey); }
        }

        /// <summary>
        /// Список версий конфигураций
        /// </summary>
        public IEnumerable<Tuple<string, string>> ConfigurationVersions
        {
            get
            {
                var result = new List<Tuple<string, string>>();
                foreach (var metadataConfiguration in Configurations)
                {
                    if (
                        result.Any(
                            c =>
                                c.Item1 == metadataConfiguration.ConfigurationId &&
                                c.Item2 == metadataConfiguration.Version))
                    {
                        continue;
                    }
                    result.Add(new Tuple<string, string>(metadataConfiguration.ConfigurationId,
                        metadataConfiguration.Version));
                }

                return result;
            }
        }

        /// <summary>
        /// Удалить указанную конфигурацию метаданных из списка загруженных конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        public void RemoveConfiguration(string metadataConfigurationId)
        {
            var configToRemove = Configurations.FirstOrDefault(c => c.ConfigurationId.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant());

            if (configToRemove != null)
            {
                RemoveConfiguration(configToRemove);
            }
        }

        /// <summary>
        /// Получить метаданные конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId)
        {
            return Configurations.FirstOrDefault(c => c.ConfigurationId.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant());
        }

        /// <summary>
        /// Добавить конфигурацию метаданных
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
        /// <param name="actionConfiguration">Конфигурация скриптовых модулей</param>
        /// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
        /// <returns>Конфигурация метаданных</returns>
        public IMetadataConfiguration AddConfiguration(string metadataConfigurationId, IScriptConfiguration actionConfiguration, bool isEmbeddedConfiguration)
        {
            var configurationExists = Configurations.FirstOrDefault(c => c.ConfigurationId == metadataConfigurationId);

            if (configurationExists != null)
            {
                return configurationExists;
            }


            var metadataConfiguration = new MetadataConfiguration(actionConfiguration,
                _serviceRegistrationContainerFactory.BuildServiceRegistrationContainer(metadataConfigurationId),
                _serviceTemplateConfiguration, isEmbeddedConfiguration)
                                        {
                                            ConfigurationId = metadataConfigurationId,
                                            Version = null
                                        };

            AddConfiguration(metadataConfiguration);

            return metadataConfiguration;
        }

        private void RemoveConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            lock (_configLock)
            {
                _sharedCacheComponent.Lock();
                try
                {
                    var configList = Configurations;
                    configList.RemoveItem(metadataConfiguration);
                    _sharedCacheComponent.Set(CacheConfigListKey, configList);
                }
                finally
                {
                    _sharedCacheComponent.Unlock();
                }
            }
        }

        private void AddConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            lock (_configLock)
            {
                _sharedCacheComponent.Lock();
                try
                {
                    var configList = Configurations;
                    configList.AddItem(metadataConfiguration);
                    _sharedCacheComponent.Set(CacheConfigListKey, configList);
                }
                finally
                {
                    _sharedCacheComponent.Unlock();
                }
            }
        }
    }
}
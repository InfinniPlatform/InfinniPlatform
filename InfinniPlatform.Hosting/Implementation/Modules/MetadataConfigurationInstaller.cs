using InfinniPlatform.Core.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Hosting.Implementation.Modules
{
    /// <summary>
    /// Базовый класс модуля конфигурации на основе метаданных
    /// </summary>
    public abstract class MetadataConfigurationInstaller : IModuleInstaller
    {
        protected MetadataConfigurationInstaller(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }

        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

        /// <summary>
        /// Установить модуль приложения
        /// </summary>
        public IModule InstallModule()
        {
            var configuration = _metadataConfigurationProvider.AddConfiguration(ConfigurationId, true);
            RegisterConfiguration(configuration);
            RegisterServices(configuration.ServiceRegistrationContainer);
            return configuration;
        }

        protected abstract string ConfigurationId { get; }

        /// <summary>
        /// Установка конфигурации метаданных
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        protected abstract void RegisterConfiguration(IMetadataConfiguration metadataConfiguration);

        /// <summary>
        /// Установить сервисы конфигурации
        /// </summary>
        /// <param name="servicesConfiguration">Контейнер регистрации сервисов</param>
        protected abstract void RegisterServices(IServiceRegistrationContainer servicesConfiguration);
    }
}
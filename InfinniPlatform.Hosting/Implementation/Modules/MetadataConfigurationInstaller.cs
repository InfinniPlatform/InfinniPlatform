using System.Text.RegularExpressions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Metadata;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Hosting.Implementation.Modules
{
    /// <summary>
    ///     Базовый класс модуля конфигурации на основе метаданных
    /// </summary>
    public abstract class MetadataConfigurationInstaller : IModuleInstaller
    {
        private readonly IScriptConfiguration _actionConfiguration;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

        protected MetadataConfigurationInstaller(IMetadataConfigurationProvider metadataConfigurationProvider,
            IScriptConfiguration actionConfiguration)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
            _actionConfiguration = actionConfiguration;
        }

        /// <summary>
        ///     Установить модуль приложения
        /// </summary>
        public IModule InstallModule()
        {
            _actionConfiguration.ModuleName = ModuleName;
            var configuration = _metadataConfigurationProvider.AddConfiguration(null, ModuleName, _actionConfiguration,
                true);
            RegisterConfiguration(configuration);
            RegisterServices(configuration.ServiceRegistrationContainer);
            return configuration;
        }

        /// <summary>
        ///     Является ли конфигурация системной
        /// </summary>
        /// <returns>Признак системной конфигурации</returns>
        public virtual bool IsSystem
        {
            get { return false; }
        }

        public string ModuleName
        {
            get { return Regex.Replace(GetType().Name, "Installer", "", RegexOptions.IgnoreCase); }
        }

        /// <summary>
        ///     Установка конфигурации метаданных
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        protected abstract void RegisterConfiguration(IMetadataConfiguration metadataConfiguration);

        /// <summary>
        ///     Установить сервисы конфигурации
        /// </summary>
        /// <param name="servicesConfiguration">Контейнер регистрации сервисов</param>
        protected abstract void RegisterServices(IServiceRegistrationContainer servicesConfiguration);
    }
}
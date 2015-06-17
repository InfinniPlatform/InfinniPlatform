using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Metadata;
using InfinniPlatform.Modules;

namespace InfinniPlatform.Hosting.Implementation.Modules
{
    /// <summary>
    ///   Базовый класс модуля конфигурации на основе метаданных
    /// </summary>
    public abstract class MetadataConfigurationInstaller : IModuleInstaller
    {
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly IScriptConfiguration _actionConfiguration;

        protected MetadataConfigurationInstaller(IMetadataConfigurationProvider metadataConfigurationProvider, IScriptConfiguration actionConfiguration)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
            _actionConfiguration = actionConfiguration;
        }


        /// <summary>
        ///  Установить модуль приложения
        /// </summary>
        public IModule InstallModule()
        {
            _actionConfiguration.ModuleName = ModuleName;
            var configuration = _metadataConfigurationProvider.AddConfiguration(null, ModuleName, _actionConfiguration, true);
            RegisterConfiguration(configuration);
            RegisterServices(configuration.ServiceRegistrationContainer);
            return configuration;
        }

        /// <summary>
        ///   Установка конфигурации метаданных 
        /// </summary>
        /// <param name="metadataConfiguration">Конфигурация метаданных</param>
        protected abstract void RegisterConfiguration(IMetadataConfiguration metadataConfiguration);

        /// <summary>
        ///   Установить сервисы конфигурации
        /// </summary>
        /// <param name="servicesConfiguration">Контейнер регистрации сервисов</param>
        protected abstract void RegisterServices(IServiceRegistrationContainer servicesConfiguration);

        /// <summary>
        ///   Является ли конфигурация системной
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
    }


}

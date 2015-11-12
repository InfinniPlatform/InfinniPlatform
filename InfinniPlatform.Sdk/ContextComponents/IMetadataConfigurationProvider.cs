using System;
using System.Collections.Generic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    ///     Провайдер метаданных конфигураций
    /// </summary>
    public interface IMetadataConfigurationProvider
    {
        /// <summary>
        ///     Список конфигураций метаданных
        /// </summary>
        IEnumerable<IMetadataConfiguration> Configurations { get; }

        /// <summary>
        /// Список версий конфигураций
        /// </summary>
        IEnumerable<Tuple<string, string>> ConfigurationVersions { get; }

        /// <summary>
        ///     Получить метаданные конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId);

        /// <summary>
        ///     Добавить конфигурацию метаданных
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
        /// <param name="actionConfiguration">Конфигурация скриптовых модулей</param>
        /// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
        /// <returns>Конфигурация метаданных</returns>
        IMetadataConfiguration AddConfiguration(string metadataConfigurationId, IScriptConfiguration actionConfiguration, bool isEmbeddedConfiguration);

        /// <summary>
        ///     Удалить указанную конфигурацию метаданных из списка загруженных конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        void RemoveConfiguration(string metadataConfigurationId);
    }
}
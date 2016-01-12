using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Провайдер метаданных конфигураций
    /// </summary>
    public interface IMetadataConfigurationProvider
    {
        /// <summary>
        /// Список конфигураций метаданных
        /// </summary>
        IEnumerable<IMetadataConfiguration> Configurations { get; }

        /// <summary>
        /// Получить метаданные конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        IMetadataConfiguration GetMetadataConfiguration(string metadataConfigurationId);

        /// <summary>
        /// Добавить конфигурацию метаданных
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
        /// <param name="isEmbeddedConfiguration">Признак встроенной в код конфигурации C#</param>
        /// <returns>Конфигурация метаданных</returns>
        IMetadataConfiguration AddConfiguration(string metadataConfigurationId, bool isEmbeddedConfiguration);

        /// <summary>
        /// Удалить указанную конфигурацию метаданных из списка загруженных конфигурации
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        void RemoveConfiguration(string metadataConfigurationId);
    }
}
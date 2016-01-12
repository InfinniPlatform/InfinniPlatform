using System;
using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Контракт для получения метаданных из контекста
    /// </summary>
    public interface IMetadataComponent
    {
        /// <summary>
        /// Получить метаданные конфигурации
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор документа</param>
        /// <param name="metadataType">Идентификатор типа метаданных</param>
        /// <param name="metadataName">Идентификатор метаданных</param>
        /// <returns>Метаданные указанного типа</returns>
        dynamic GetMetadata(string configId, string objectMetadataId, string metadataType, string metadataName);

        /// <summary>
        /// Получить список всех конфигураций
        /// </summary>
        /// <returns>Список метаданных конфигураций</returns>
        dynamic GetConfigMetadata();

        /// <summary>
        /// Получить метаданные конфигурации
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор документа</param>
        /// <param name="metadataType">Тип метаданных</param>
        /// <param name="predicate">Предикат для получения метаданных</param>
        dynamic GetMetadataItem(string configId, string objectMetadataId, string metadataType, Func<object, bool> predicate);

        /// <summary>
        /// Получить метаданные первого уровня вложенности (регистр)
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="objectMetadataId">Идентификатор объекта метаданных (регистр)</param>
        /// <param name="metadataType">Тип объекта метаданных (MetadataType.Register)</param>
        /// <returns>Метаданные объекта первого уровня вложенности</returns>
        IEnumerable<dynamic> GetMetadataList(string configId, string objectMetadataId, string metadataType);
    }
}
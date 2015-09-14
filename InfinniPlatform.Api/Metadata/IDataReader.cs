using System.Collections.Generic;

namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    ///     Контракт провайдера запросов к метаданным конфигурации
    /// </summary>
    public interface IDataReader
    {
        /// <summary>
        ///     Получить метаданные объекта в кратком виде (ссылки на метаданные объектов конфигурации)
        /// </summary>
        /// <returns>Список описаний метаданных объекта в кратком формате</returns>
        IEnumerable<dynamic> GetItems();

        /// <summary>
        ///     Получить метаданные конкретного объекта
        /// </summary>
        /// <param name="metadataName">наименование объекта</param>
        /// <returns>Метаданные объекта конфигурации</returns>
        dynamic GetItem(string metadataName);
    }
}
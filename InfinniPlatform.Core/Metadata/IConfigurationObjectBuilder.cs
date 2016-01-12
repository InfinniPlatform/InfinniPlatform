using System.Collections.Generic;

namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Конструктор контекста конфигурации для использования в прикладных скриптах
    /// </summary>
    public interface IConfigurationObjectBuilder
    {
        /// <summary>
        /// Получить объект конфигурации для указанного идентификатора
        /// </summary>
        /// <param name="metadataIdentifier">Идентификатор метаданных</param>
        /// <returns>Объект конфигурации метаданных</returns>
        IConfigurationObject GetConfigurationObject(string metadataIdentifier);

        /// <summary>
        /// Получить список зарегистрированных конфигураций
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMetadataConfiguration> GetConfigurationList();
    }
}
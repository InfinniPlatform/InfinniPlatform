using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Metadata
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

        /// <summary>
        /// Получить список соответствия конфигураций и версий, существующих в системе
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<string, string>> GetConfigurationVersions();
    }
}
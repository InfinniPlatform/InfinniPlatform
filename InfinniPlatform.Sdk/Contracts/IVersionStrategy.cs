using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    ///   Стратегия версионирования запросов к сервисам
    /// </summary>
    public interface IVersionStrategy
    {
        /// <summary>
        /// Получить актуальную минорную версию конфигурации для указанного идентификатора версии
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <param name="configurationVersions">Список версий конфигураций</param>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Актуальная минорная версия конфигурации</returns>
        string GetActualVersion(string metadataConfigurationId, IEnumerable<Tuple<string, string>> configurationVersions, string userName);

        /// <summary>
        ///   Получить список неактуальных версий
        /// </summary>
        /// <returns>Список соответствия актуальных и неактуальных версий конфигураций</returns>
        IEnumerable<object> GetIrrelevantVersionList(IEnumerable<Tuple<string, string>> configurationVersions, string userName);
    }
}

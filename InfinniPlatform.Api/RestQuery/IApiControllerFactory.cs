namespace InfinniPlatform.Api.RestQuery
{
    /// <summary>
    /// Factory for creating instances of REST service
    /// </summary>
    public interface IApiControllerFactory
    {
        /// <summary>
        /// Получить шаблон исполняемого действия
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <param name="metadataName">Наименование метаданных</param>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Контейнер, содержащий шаблон действия</returns>
        IRestVerbsContainer GetTemplate(string metadataConfigurationId, string metadataName, string userName);

        /// <summary>
        /// Удалить зарегистрированные шаблоны
        /// </summary>
        /// <param name="version">Версия конфигурации, для которой следует удалить шаблоны</param>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        void RemoveTemplates(string version, string metadataConfigurationId);
    }
}
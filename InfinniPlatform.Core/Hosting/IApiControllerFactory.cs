namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    /// Фабрика для построения REST-сервисов.
    /// </summary>
    public interface IApiControllerFactory
    {
        /// <summary>
        /// Создает объект для регистрации сервисов документа.
        /// </summary>
        /// <param name="configId">Имя конфигурации.</param>
        /// <param name="documentType">Имя типа документа.</param>
        IRestVerbsRegistrator CreateTemplate(string configId, string documentType);

        /// <summary>
        /// Возвращает объект для поиска сервиса документа.
        /// </summary>
        /// <param name="configId">Имя конфигурации.</param>
        /// <param name="documentType">Имя типа документа.</param>
        IRestVerbsContainer GetTemplate(string configId, string documentType);

        /// <summary>
        /// Удаляет зарегистрированные сервисы всех документов конфигурации.
        /// </summary>
        /// <param name="configId">Имя конфигурации.</param>
        void RemoveTemplates(string configId);
    }
}
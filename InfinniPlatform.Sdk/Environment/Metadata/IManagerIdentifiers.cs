namespace InfinniPlatform.Sdk.Environment.Metadata
{
    /// <summary>
    ///     Менеджер для работы с идентификаторами метаданных
    /// </summary>
    public interface IManagerIdentifiers
    {
        /// <summary>
        ///     Получить идентификатор элемента конфигурации
        /// </summary>
        /// <param name="version"></param>
        /// <param name="name">Наименование элемента</param>
        /// <returns>Идентификатор элемента</returns>
        string GetConfigurationUid(string version, string name);

        string GetDocumentUid(string version, string configurationId, string documentId);

        /// <summary>
        ///   Получить идентификатор метаданных решения
        /// </summary>
        /// <param name="version">Версия метаданных решения</param>
        /// <param name="name">Наименование решения</param>
        /// <returns>Идентификатор решения</returns>
        string GetSolutionUid(string version, string name);
    }
}
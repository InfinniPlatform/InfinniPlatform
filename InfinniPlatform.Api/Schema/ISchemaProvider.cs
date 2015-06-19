namespace InfinniPlatform.Api.Schema
{
    /// <summary>
    ///     Провайдер схемы документа
    /// </summary>
    public interface ISchemaProvider
    {
        /// <summary>
        ///     Получить схему документа
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Схема документа</returns>
        dynamic GetSchema(string version, string configId, string documentId);
    }
}
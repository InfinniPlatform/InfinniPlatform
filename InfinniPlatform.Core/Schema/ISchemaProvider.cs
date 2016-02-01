namespace InfinniPlatform.Core.Schema
{
    /// <summary>
    ///     Провайдер схемы документа
    /// </summary>
    public interface ISchemaProvider
    {
        /// <summary>
        ///     Получить схему документа
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Схема документа</returns>
        dynamic GetSchema(string configId, string documentId);
    }
}
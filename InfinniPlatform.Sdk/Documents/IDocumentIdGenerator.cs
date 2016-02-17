namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Предоставляет интерфейс для генерации уникального идентификатора документа.
    /// </summary>
    public interface IDocumentIdGenerator
    {
        /// <summary>
        /// Создает новый уникальный идентификатор.
        /// </summary>
        object NewId();
    }
}
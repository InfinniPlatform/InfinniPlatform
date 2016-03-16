namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на удаление документа.
    /// </summary>
    public class DocumentDeleteQueryResult : DocumentQeuryResult
    {
        /// <summary>
        /// Количество удаленных документов.
        /// </summary>
        public long? DeletedCount { get; set; }
    }
}
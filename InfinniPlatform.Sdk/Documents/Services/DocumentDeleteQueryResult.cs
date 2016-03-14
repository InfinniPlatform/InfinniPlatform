namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на удаление документа.
    /// </summary>
    public class DocumentDeleteQueryResult
    {
        /// <summary>
        /// Количество удаленных документов.
        /// </summary>
        public long? DeletedCount { get; set; }

        /// <summary>
        /// Результат выполнения проверки корректности документа.
        /// </summary>
        public DocumentValidationResult Validation { get; set; }
    }
}
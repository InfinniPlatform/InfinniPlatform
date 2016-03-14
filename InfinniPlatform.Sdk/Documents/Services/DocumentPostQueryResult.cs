namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Результат выполнения запроса на сохранение документа.
    /// </summary>
    public class DocumentPostQueryResult
    {
        /// <summary>
        /// Результат выполнения обновления документа.
        /// </summary>
        public DocumentUpdateResult Status { get; set; }

        /// <summary>
        /// Результат выполнения проверки корректности документа.
        /// </summary>
        public DocumentValidationResult Validation { get; set; }
    }
}
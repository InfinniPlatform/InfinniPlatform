namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Базовый класс результата выполнения запроса.
    /// </summary>
    public abstract class DocumentQueryResult
    {
        /// <summary>
        /// Результат выполнения проверки корректности выполнения запроса.
        /// </summary>
        public DocumentValidationResult ValidationResult { get; set; }
    }
}
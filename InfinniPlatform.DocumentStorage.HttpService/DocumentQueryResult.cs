namespace InfinniPlatform.DocumentStorage
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

        /// <summary>
        /// Дополнительные данные результата выполнения запроса.
        /// </summary>
        public object AdditionalResult { get; set; }
    }
}
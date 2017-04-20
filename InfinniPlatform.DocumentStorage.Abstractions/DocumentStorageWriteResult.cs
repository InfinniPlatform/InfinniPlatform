namespace InfinniPlatform.DocumentStorage
{
    public abstract class DocumentStorageWriteResult
    {
        /// <summary>
        /// Успешность выполнения команды.
        /// </summary>
        public bool Success => (ValidationResult == null || ValidationResult.IsValid);

        /// <summary>
        /// Сообщение об ошибке выполнения команды.
        /// </summary>
        public DocumentValidationResult ValidationResult { get; set; }
    }
}
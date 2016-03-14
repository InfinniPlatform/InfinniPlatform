namespace InfinniPlatform.Sdk.Documents.Interceptors
{
    /// <summary>
    /// Результат выполнения команды изменения документа в хранилище.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public sealed class DocumentStorageWriteResult<TResult>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="success">Успешность выполнения команды.</param>
        public DocumentStorageWriteResult(bool success = true)
        {
            Success = success;
        }


        /// <summary>
        /// Успешность выполнения команды.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Результат выполнения команды.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Сообщение об ошибке выполнения команды.
        /// </summary>
        public DocumentValidationResult ValidationResult { get; set; }
    }
}
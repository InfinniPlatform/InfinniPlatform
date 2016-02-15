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
        /// <param name="isValid">Успешность выполнения команды.</param>
        public DocumentStorageWriteResult(bool isValid = true)
        {
            IsValid = isValid;
        }


        /// <summary>
        /// Успешность выполнения команды.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Результат выполнения команды.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Сообщение об ошибке выполнения команды.
        /// </summary>
        public object ValidationResult { get; set; }
    }
}
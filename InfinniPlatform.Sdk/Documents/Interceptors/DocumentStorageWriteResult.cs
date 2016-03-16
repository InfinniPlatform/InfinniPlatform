namespace InfinniPlatform.Sdk.Documents.Interceptors
{
    /// <summary>
    /// Результат выполнения команды изменения документа в хранилище.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public sealed class DocumentStorageWriteResult<TResult> : DocumentStorageWriteResult
    {
        /// <summary>
        /// Результат выполнения команды.
        /// </summary>
        public TResult Result { get; set; }
    }
}
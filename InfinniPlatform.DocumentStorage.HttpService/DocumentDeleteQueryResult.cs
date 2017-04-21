namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Результат выполнения запроса на удаление документа.
    /// </summary>
    public class DocumentDeleteQueryResult : DocumentQueryResult
    {
        /// <summary>
        /// Количество удаленных документов.
        /// </summary>
        public long? DeletedCount { get; set; }
    }
}
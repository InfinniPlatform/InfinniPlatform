namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Результат выполнения операции массового изменения документов.
    /// </summary>
    public sealed class DocumentBulkResult
    {
        public DocumentBulkResult(int requestCount, long matchedCount, long insertedCount, long modifiedCount, long deletedCount)
        {
            RequestCount = requestCount;
            MatchedCount = matchedCount;
            InsertedCount = insertedCount;
            ModifiedCount = modifiedCount;
            DeletedCount = deletedCount;
        }

        /// <summary>
        /// Количество обработанных запросов.
        /// </summary>
        public int RequestCount { get; }

        /// <summary>
        /// Количество найденных документов.
        /// </summary>
        public long MatchedCount { get; }

        /// <summary>
        /// Количество вставленных документов.
        /// </summary>
        public long InsertedCount { get; }

        /// <summary>
        /// Количество измененных документов.
        /// </summary>
        public long ModifiedCount { get; }

        /// <summary>
        /// Количество удаленных документов.
        /// </summary>
        public long DeletedCount { get; }
    }
}
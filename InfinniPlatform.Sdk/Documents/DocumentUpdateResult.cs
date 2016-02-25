namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Результат выполнения операции обновления документов.
    /// </summary>
    public sealed class DocumentUpdateResult
    {
        public DocumentUpdateResult(long matchedCount, long modifiedCount, DocumentUpdateStatus updateStatus)
        {
            MatchedCount = matchedCount;
            ModifiedCount = modifiedCount;
            UpdateStatus = updateStatus;
        }

        /// <summary>
        /// Количество найденных документов.
        /// </summary>
        public long MatchedCount { get; }

        /// <summary>
        /// Количество измененных документов.
        /// </summary>
        public long ModifiedCount { get; }

        /// <summary>
        /// Статус выполнения операции обновления документа.
        /// </summary>
        public DocumentUpdateStatus UpdateStatus { get; }
    }
}
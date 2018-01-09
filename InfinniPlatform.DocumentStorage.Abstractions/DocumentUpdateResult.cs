namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Result of document storage update command.
    /// </summary>
    public sealed class DocumentUpdateResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DocumentUpdateResult" />.
        /// </summary>
        /// <param name="matchedCount">Number of found documents.</param>
        /// <param name="modifiedCount">Number of modified documents.</param>
        /// <param name="updateStatus">Status of update operation.</param>
        public DocumentUpdateResult(long matchedCount, long modifiedCount, DocumentUpdateStatus updateStatus)
        {
            MatchedCount = matchedCount;
            ModifiedCount = modifiedCount;
            UpdateStatus = updateStatus;
        }

        /// <summary>
        /// Number of found documents.
        /// </summary>
        public long MatchedCount { get; }

        /// <summary>
        /// Number of modified documents.
        /// </summary>
        public long ModifiedCount { get; }

        /// <summary>
        /// Status of update operation.
        /// </summary>
        public DocumentUpdateStatus UpdateStatus { get; }
    }
}
namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Result of bulk document operation.
    /// </summary>
    public sealed class DocumentBulkResult
    {
        /// <summary>
        /// Default instance of <see cref="DocumentBulkResult"/>.
        /// </summary>
        public static readonly DocumentBulkResult Empty = new DocumentBulkResult(0, 0, 0, 0, 0);


        /// <summary>
        /// Initializes a new instance of <see cref="DocumentBulkResult" />.
        /// </summary>
        /// <param name="requestCount">Number of requests processed.</param>
        /// <param name="matchedCount">Number of matched documents.</param>
        /// <param name="insertedCount">Number of inserted documents.</param>
        /// <param name="modifiedCount">Number of modified documents.</param>
        /// <param name="deletedCount">Number of deleted documents.</param>
        public DocumentBulkResult(int requestCount, long matchedCount, long insertedCount, long modifiedCount, long deletedCount)
        {
            RequestCount = requestCount;
            MatchedCount = matchedCount;
            InsertedCount = insertedCount;
            ModifiedCount = modifiedCount;
            DeletedCount = deletedCount;
        }


        /// <summary>
        /// Number of requests processed.
        /// </summary>
        public int RequestCount { get; }

        /// <summary>
        /// Number of matched documents.
        /// </summary>
        public long MatchedCount { get; }

        /// <summary>
        /// Number of inserted documents.
        /// </summary>
        public long InsertedCount { get; }

        /// <summary>
        /// Number of modified documents.
        /// </summary>
        public long ModifiedCount { get; }

        /// <summary>
        /// Number of deleted documents.
        /// </summary>
        public long DeletedCount { get; }
    }
}
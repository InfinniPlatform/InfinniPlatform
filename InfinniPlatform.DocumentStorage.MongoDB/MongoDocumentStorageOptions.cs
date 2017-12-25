namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// MongoDB document storage settings configuration options. 
    /// </summary>
    public class MongoDocumentStorageOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "mongodbDocumentStorage";

        /// <summary>
        /// Default instance of <see cref="MongoDocumentStorageOptions" />.
        /// </summary>
        public static MongoDocumentStorageOptions Default = new MongoDocumentStorageOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="MongoDocumentStorageOptions"/>.
        /// </summary>
        public MongoDocumentStorageOptions()
        {
            ConnectionString = "mongodb://localhost:27017";
        }

        /// <summary>
        /// Connection string.
        /// </summary>
        /// <remarks>
        /// Connection string format could be found at https://docs.mongodb.com/manual/reference/connection-string.
        /// </remarks>
        public string ConnectionString { get; set; }
    }
}
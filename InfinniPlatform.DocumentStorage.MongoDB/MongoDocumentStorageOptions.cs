namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Настройки хранилища документов MongoDB.
    /// </summary>
    public class MongoDocumentStorageOptions
    {
        public const string SectionName = "mongodbDocumentStorage";

        public static MongoDocumentStorageOptions Default = new MongoDocumentStorageOptions();


        public MongoDocumentStorageOptions()
        {
            ConnectionString = "localhost:27017";
        }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        /// <remarks>
        /// Подробнее см. https://docs.mongodb.com/manual/reference/connection-string..
        /// </remarks>
        public string ConnectionString { get; set; }
    }
}
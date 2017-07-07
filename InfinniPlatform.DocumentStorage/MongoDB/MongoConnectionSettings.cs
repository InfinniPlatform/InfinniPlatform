namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Настройки хранилища документов MongoDB.
    /// </summary>
    internal sealed class MongoConnectionSettings
    {
        public const string SectionName = "mongodb";

        public static MongoConnectionSettings Default = new MongoConnectionSettings();


        public MongoConnectionSettings()
        {
            ConnectionString = "mongodb://localhost:27017";
        }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        /// <remarks>
        /// Подробнее см. https://docs.mongodb.com/manual/reference/connection-string.
        /// </remarks>
        public string ConnectionString { get; set; }
    }
}
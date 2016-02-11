namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Настройки подключения к MongoDB.
    /// </summary>
    internal sealed class MongoConnectionSettings
    {
        public const string SectionName = "mongodb";

        public static MongoConnectionSettings Default = new MongoConnectionSettings();


        public MongoConnectionSettings()
        {
            Nodes = new[] { "localhost:27017" };
        }


        /// <summary>
        /// Список узлов кластера.
        /// </summary>
        /// <remarks>
        /// Каждый узел должен быть представлен в формате "host:port".
        /// </remarks>
        public string[] Nodes { get; set; }

        /// <summary>
        /// Имя пользователя в механизме аутентификации по умолчанию.
        /// </summary>
        /// <remarks>
        /// Начиная с MongoDB 3.0 по умолчанию используется SCRAM-SHA-1 вместо MONGODB-CR.
        /// </remarks>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя в механизме аутентификации по умолчанию.
        /// </summary>
        /// <remarks>
        /// Начиная с MongoDB 3.0 по умолчанию используется SCRAM-SHA-1 вместо MONGODB-CR.
        /// </remarks>
        public string Password { get; set; }
    }
}
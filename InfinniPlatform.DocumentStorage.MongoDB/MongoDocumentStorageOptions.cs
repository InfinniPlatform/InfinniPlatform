using System.Collections.Generic;

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
            Nodes = new[] { "localhost:27017" };
        }


        /// <summary>
        /// Список узлов кластера.
        /// </summary>
        /// <remarks>
        /// Каждый узел должен быть представлен в формате "host:port".
        /// </remarks>
        public IEnumerable<string> Nodes { get; set; }

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
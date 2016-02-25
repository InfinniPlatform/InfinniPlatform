using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB
{
    internal static class MongoTestHelpers
    {
        public const string DatabaseName = "MongoTest";


        public static MongoConnection GetConnection()
        {
            return new MongoConnection(DatabaseName, MongoConnectionSettings.Default);
        }


        public static MongoDocumentStorageProvider GetStorageProvider(string documentType = null)
        {
            return new MongoDocumentStorageProvider(GetConnection(), documentType);
        }

        public static MongoDocumentStorageProvider<TDocument> GetStorageProvider<TDocument>(string documentType = null)
        {
            return new MongoDocumentStorageProvider<TDocument>(GetConnection(), documentType);
        }


        public static MongoDocumentStorageProvider GetEmptyStorageProvider(string documentType, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, indexes);
            return new MongoDocumentStorageProvider(connection, documentType);
        }


        public static MongoDocumentStorageProvider<TDocument> GetEmptyStorageProvider<TDocument>(string documentType, params DocumentIndex[] indexes)
        {
            var connection = CreateEmptyStorage(documentType, indexes);
            return new MongoDocumentStorageProvider<TDocument>(connection, documentType);
        }


        private static MongoConnection CreateEmptyStorage(string documentType, params DocumentIndex[] indexes)
        {
            var connection = GetConnection();
            var database = connection.GetDatabase();
            database.DropCollection(documentType);

            if (indexes != null && indexes.Length > 0)
            {
                var storageManager = new MongoDocumentStorageManager(connection);
                storageManager.CreateStorageAsync(new DocumentMetadata { Type = documentType, Indexes = indexes }).Wait();
            }

            return connection;
        }
    }
}
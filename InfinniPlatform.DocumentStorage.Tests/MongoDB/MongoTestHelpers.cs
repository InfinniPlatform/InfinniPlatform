using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.Sdk.Metadata.Documents;

namespace InfinniPlatform.DocumentStorage.Tests.MongoDB
{
    internal static class MongoTestHelpers
    {
        public static MongoConnection GetConnection()
        {
            return new MongoConnection("MongoTest", MongoConnectionSettings.Default);
        }

        public static MongoDocumentStorageProvider GetEmptyStorage(string documentType, params DocumentIndex[] indexes)
        {
            var connection = GetConnection();
            var database = connection.GetDatabase();
            database.DropCollection(documentType);

            if (indexes != null && indexes.Length > 0)
            {
                var storageManager = new MongoDocumentStorageManager(connection);
                storageManager.CreateStorageAsync(new DocumentMetadata { Type = documentType, Indexes = indexes }).Wait();
            }

            return new MongoDocumentStorageProvider(connection, documentType);
        }
    }
}
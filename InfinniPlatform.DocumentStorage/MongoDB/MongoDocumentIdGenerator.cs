using InfinniPlatform.Sdk.Documents;

using MongoDB.Bson;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет интерфейс для генерации уникального идентификатора документа MongoDB.
    /// </summary>
    internal sealed class MongoDocumentIdGenerator : IDocumentIdGenerator
    {
        public object NewId()
        {
            return ObjectId.GenerateNewId();
        }
    }
}
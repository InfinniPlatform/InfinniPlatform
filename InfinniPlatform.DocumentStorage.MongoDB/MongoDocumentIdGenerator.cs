using InfinniPlatform.DocumentStorage.Abstractions;

using MongoDB.Bson;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет интерфейс для генерации уникального идентификатора документа MongoDB.
    /// </summary>
    internal class MongoDocumentIdGenerator : IDocumentIdGenerator
    {
        public object NewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
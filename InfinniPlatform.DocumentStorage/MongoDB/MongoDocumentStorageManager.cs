using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Metadata.Documents;

using MongoDB.Bson;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы для управления хранилищем документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentStorageManager : IDocumentStorageManager
    {
        public MongoDocumentStorageManager(MongoConnection connection)
        {
            _connection = connection;
        }


        private readonly MongoConnection _connection;


        /// <summary>
        /// Создает хранилище данных для заданного типа документа.
        /// </summary>
        /// <param name="documentMetadata">Метаданные документа.</param>
        public async Task CreateStorageAsync(DocumentMetadata documentMetadata)
        {
            var collectionName = documentMetadata.Type;

            var database = _connection.GetDatabase();

            // Создание коллекции документов, если она еще не создана
            if (!await CollectionExistsAsync(database, collectionName))
            {
                await database.CreateCollectionAsync(collectionName);
            }

            var collection = database.GetCollection<BsonDocument>(collectionName);

            // Получение актуального списка индексов коллекции документов
            var actualIndexes = await GetCollectionIndexesAsync(collection);

            // Получение требуемого списка индексов коллекции документов
            var neededIndexes = documentMetadata.Indexes ?? new DocumentIndex[] { };

            // Удаление неактуальных индексов

            var dropIndexes = actualIndexes.Except(neededIndexes).Distinct();

            foreach (var dropIndex in dropIndexes)
            {
                await collection.Indexes.DropOneAsync(dropIndex.Name);
            }

            // Создание недостающих индексов

            var createIndexes = neededIndexes.Except(actualIndexes).Distinct();

            foreach (var createIndex in createIndexes)
            {
                if (createIndex.Key != null && createIndex.Key.Count > 0)
                {
                    IndexKeysDefinition<BsonDocument> keysDefinition = null;

                    foreach (var property in createIndex.Key)
                    {
                        if (keysDefinition == null)
                        {
                            switch (property.Value)
                            {
                                case DocumentIndexKeyType.Asc:
                                    keysDefinition = Builders<BsonDocument>.IndexKeys.Ascending(property.Key);
                                    break;
                                case DocumentIndexKeyType.Desc:
                                    keysDefinition = Builders<BsonDocument>.IndexKeys.Descending(property.Key);
                                    break;
                                case DocumentIndexKeyType.Text:
                                    keysDefinition = Builders<BsonDocument>.IndexKeys.Text(property.Key);
                                    break;
                            }
                        }
                        else
                        {
                            switch (property.Value)
                            {
                                case DocumentIndexKeyType.Asc:
                                    keysDefinition = keysDefinition.Ascending(property.Key);
                                    break;
                                case DocumentIndexKeyType.Desc:
                                    keysDefinition = keysDefinition.Descending(property.Key);
                                    break;
                                case DocumentIndexKeyType.Text:
                                    keysDefinition = keysDefinition.Text(property.Key);
                                    break;
                            }
                        }
                    }

                    CreateIndexOptions indexOptions = null;

                    if (createIndex.Unique || !string.IsNullOrEmpty(createIndex.Name))
                    {
                        indexOptions = new CreateIndexOptions
                        {
                            Unique = createIndex.Unique,
                            Name = createIndex.Name
                        };
                    }

                    if (indexOptions != null)
                    {
                        await collection.Indexes.CreateOneAsync(keysDefinition, indexOptions);
                    }
                    else
                    {
                        await collection.Indexes.CreateOneAsync(keysDefinition);
                    }
                }
            }
        }

        public async Task RenameStorageAsync(string documentType, string newDocumnetType)
        {
            var database = _connection.GetDatabase();

            // Создание коллекции документов, если она еще не создана
            if (await CollectionExistsAsync(database, documentType))
            {
                await database.RenameCollectionAsync(documentType, newDocumnetType);
            }
        }

        /// <summary>
        /// Удаляет хранилище данных для заданного типа документа.
        /// </summary>
        /// <param name="documentType">Имя типа документа.</param>
        public async Task DropStorageAsync(string documentType)
        {
            var database = _connection.GetDatabase();

            await database.DropCollectionAsync(documentType);
        }


        private static async Task<bool> CollectionExistsAsync(IMongoDatabase database, string collectionName)
        {
            var collectionNameFilter = Builders<BsonDocument>.Filter.Eq("name", collectionName);

            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = collectionNameFilter });

            return collections.Any();
        }

        private static async Task<IList<DocumentIndex>> GetCollectionIndexesAsync(IMongoCollection<BsonDocument> collection)
        {
            var result = new List<DocumentIndex>();

            using (var cursor = await collection.Indexes.ListAsync())
            {
                var indexes = await cursor.ToListAsync();

                foreach (var index in indexes)
                {
                    var indexName = TryGetValue(index, "name", v => v.AsString);

                    // Не рассматриваются системные индексы
                    if (!string.Equals(indexName, "_id_", StringComparison.OrdinalIgnoreCase))
                    {
                        var indexUnique = TryGetValue(index, "unique", v => v.AsBoolean);
                        var indexKey = TryGetValue(index, "key", v => v.AsBsonDocument);

                        var indexMetadata = new DocumentIndex
                        {
                            Name = indexName,
                            Unique = indexUnique,
                            Key = new Dictionary<string, DocumentIndexKeyType>()
                        };

                        var knowKeyType = true;

                        if (indexKey != null)
                        {
                            foreach (var property in indexKey)
                            {
                                var keyType = property.Value;

                                if (keyType == 1)
                                {
                                    indexMetadata.Key[property.Name] = DocumentIndexKeyType.Asc;
                                }
                                else if (keyType == -1)
                                {
                                    indexMetadata.Key[property.Name] = DocumentIndexKeyType.Desc;
                                }
                                else if (keyType == "text")
                                {
                                    indexMetadata.Key[property.Name] = DocumentIndexKeyType.Text;
                                }
                                else
                                {
                                    // Не рассматриваются индексы с неизвестным типом индексации
                                    knowKeyType = false;
                                    break;
                                }
                            }
                        }

                        if (knowKeyType && indexMetadata.Key.Count > 0)
                        {
                            result.Add(indexMetadata);
                        }
                    }
                }
            }

            return result;
        }


        private static T TryGetValue<T>(BsonDocument document, string property, Func<BsonValue, T> selector)
        {
            BsonValue value;

            return (document.TryGetValue(property, out value) && value != null) ? selector(value) : default(T);
        }
    }
}
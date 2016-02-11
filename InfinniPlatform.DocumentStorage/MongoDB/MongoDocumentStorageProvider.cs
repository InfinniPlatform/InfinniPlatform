using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы для управления данными хранилища документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentStorageProvider : IDocumentStorageProvider
    {
        public MongoDocumentStorageProvider(MongoConnection connection, string documentType)
        {
            _collection = new Lazy<IMongoCollection<DynamicWrapper>>(() => connection.GetDatabase().GetCollection<DynamicWrapper>(documentType));
            _filterBuilder = new MongoDocumentFilterBuilder<DynamicWrapper>();
        }


        private readonly Lazy<IMongoCollection<DynamicWrapper>> _collection;
        private readonly MongoDocumentFilterBuilder<DynamicWrapper> _filterBuilder;


        public long Count(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _collection.Value.Count(_filterBuilder.CreateMongoFilter(filter));
        }

        public Task<long> CountAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return _collection.Value.CountAsync(_filterBuilder.CreateMongoFilter(filter));
        }


        public IDocumentCursor<TProperty> Distinct<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = _collection.Value.Distinct<TProperty>(property, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TProperty>(result);
        }

        public async Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(string property, Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = await _collection.Value.DistinctAsync<TProperty>(property, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TProperty>(result);
        }


        public IDocumentFindCursor Find(Func<IDocumentFilterBuilder, object> filter = null)
        {
            return new MongoDocumentFindCursor(_collection, _filterBuilder.CreateMongoFilter(filter));
        }


        public IDocumentAggregateCursor Aggregate(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var fluentAggregateCursor = _collection.Value.Aggregate();

            if (filter != null)
            {
                fluentAggregateCursor = fluentAggregateCursor.Match(_filterBuilder.CreateMongoFilter(filter));
            }

            return new MongoDocumentAggregateCursor(fluentAggregateCursor);
        }


        public void InsertOne(DynamicWrapper document)
        {
            _collection.Value.InsertOne(document);
        }

        public Task InsertOneAsync(DynamicWrapper document)
        {
            return _collection.Value.InsertOneAsync(document);
        }

        public void InsertMany(IEnumerable<DynamicWrapper> documents)
        {
            _collection.Value.InsertMany(documents);
        }

        public Task InsertManyAsync(IEnumerable<DynamicWrapper> documents)
        {
            return _collection.Value.InsertManyAsync(documents);
        }


        public DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateOne(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateOneAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateMany(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateManyAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public DocumentUpdateResult ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.ReplaceOne(_filterBuilder.CreateMongoFilter(filter), replacement, new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateReplaceResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> ReplaceOneAsync(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.ReplaceOneAsync(_filterBuilder.CreateMongoFilter(filter), replacement, new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateReplaceResult(result, insertIfNotExists);
        }

        public long DeleteOne(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = _collection.Value.DeleteOne(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public async Task<long> DeleteOneAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = await _collection.Value.DeleteOneAsync(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public long DeleteMany(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = _collection.Value.DeleteMany(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public async Task<long> DeleteManyAsync(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var result = await _collection.Value.DeleteManyAsync(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }


        public DocumentBulkResult Bulk(Action<IDocumentBulkBuilder> bulk, bool isOrdered = false)
        {
            var requests = MongoDocumentBulkBuilder.CreateMongoBulk(_filterBuilder, bulk);
            var result = _collection.Value.BulkWrite(requests, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }

        public async Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder> bulk, bool isOrdered = false)
        {
            var requests = MongoDocumentBulkBuilder.CreateMongoBulk(_filterBuilder, bulk);
            var result = await _collection.Value.BulkWriteAsync(requests, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет низкоуровневые методы для работы с данными хранилища документов в MongoDB.
    /// </summary>
    internal class MongoDocumentStorageProvider : IDocumentStorageProvider
    {
        public MongoDocumentStorageProvider(MongoConnection connection, string documentType)
        {
            DocumentType = documentType;

            _collection = new Lazy<IMongoCollection<DynamicDocument>>(() => connection.GetDatabase().GetCollection<DynamicDocument>(documentType));
            _filterBuilder = new MongoDocumentFilterBuilder<DynamicDocument>();
        }


        private readonly Lazy<IMongoCollection<DynamicDocument>> _collection;
        private readonly MongoDocumentFilterBuilder<DynamicDocument> _filterBuilder;


        public string DocumentType { get; }


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
            var findCursor = new MongoDocumentFindCursor(_collection, _filterBuilder);

            findCursor.Where(filter);

            return findCursor;
        }

        public IDocumentFindCursor FindText(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false, Func<IDocumentFilterBuilder, object> filter = null)
        {
            var findCursor = new MongoDocumentFindCursor(_collection, _filterBuilder);

            if (string.IsNullOrWhiteSpace(search))
            {
                findCursor.Where(filter);
            }
            else
            {
                findCursor.Where(f => f.Text(search, language, caseSensitive, diacriticSensitive));

                if (filter != null)
                {
                    findCursor.Where(filter);
                }
            }

            return findCursor;
        }


        public IDocumentAggregateCursor Aggregate(Func<IDocumentFilterBuilder, object> filter = null)
        {
            var aggregateCursor = _collection.Value.Aggregate();

            if (filter != null)
            {
                aggregateCursor = aggregateCursor.Match(_filterBuilder.CreateMongoFilter(filter));
            }

            return new MongoDocumentAggregateCursor(aggregateCursor);
        }


        public void InsertOne(DynamicDocument document)
        {
            _collection.Value.InsertOne(document);
        }

        public Task InsertOneAsync(DynamicDocument document)
        {
            return _collection.Value.InsertOneAsync(document);
        }

        public void InsertMany(IEnumerable<DynamicDocument> documents)
        {
            _collection.Value.InsertMany(documents);
        }

        public Task InsertManyAsync(IEnumerable<DynamicDocument> documents)
        {
            return _collection.Value.InsertManyAsync(documents);
        }


        public DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateOne(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateOneAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateMany(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateManyAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public DocumentUpdateResult ReplaceOne(DynamicDocument replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.ReplaceOne(_filterBuilder.CreateMongoFilter(filter), replacement, new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateReplaceResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> ReplaceOneAsync(DynamicDocument replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
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


        public DocumentBulkResult Bulk(Action<IDocumentBulkBuilder> requests, bool isOrdered = false)
        {
            var bulk = MongoDocumentBulkBuilder.CreateMongoBulk(_filterBuilder, requests);
            var result = _collection.Value.BulkWrite(bulk, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }

        public async Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder> requests, bool isOrdered = false)
        {
            var bulk = MongoDocumentBulkBuilder.CreateMongoBulk(_filterBuilder, requests);
            var result = await _collection.Value.BulkWriteAsync(bulk, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }
    }
}
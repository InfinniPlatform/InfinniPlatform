using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет низкоуровневые методы для работы с данными хранилища документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentStorageProvider<TDocument> : IDocumentStorageProvider<TDocument>
    {
        public MongoDocumentStorageProvider(MongoConnection connection, string documentType = null)
        {
            if (string.IsNullOrEmpty(documentType))
            {
                documentType = DocumentStorageExtensions.GetDefaultDocumentTypeName<TDocument>();
            }

            DocumentType = documentType;

            _database = new Lazy<IMongoDatabase>(connection.GetDatabase);
            _collection = new Lazy<IMongoCollection<TDocument>>(() => _database.Value.GetCollection<TDocument>(documentType));
            _filterBuilder = new MongoDocumentFilterBuilder<TDocument>();
        }


        private readonly Lazy<IMongoDatabase> _database;
        private readonly Lazy<IMongoCollection<TDocument>> _collection;
        private readonly MongoDocumentFilterBuilder<TDocument> _filterBuilder;


        public string DocumentType { get; }


        public long Count(Expression<Func<TDocument, bool>> filter = null)
        {
            return _collection.Value.Count(_filterBuilder.CreateMongoFilter(filter));
        }

        public Task<long> CountAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            return _collection.Value.CountAsync(_filterBuilder.CreateMongoFilter(filter));
        }


        public IDocumentCursor<TProperty> Distinct<TProperty>(Expression<Func<TDocument, TProperty>> property, Expression<Func<TDocument, bool>> filter = null)
        {
            var result = _collection.Value.Distinct(property, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TProperty>(result);
        }

        public async Task<IDocumentCursor<TProperty>> DistinctAsync<TProperty>(Expression<Func<TDocument, TProperty>> property, Expression<Func<TDocument, bool>> filter = null)
        {
            var result = await _collection.Value.DistinctAsync(property, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TProperty>(result);
        }


        public IDocumentCursor<TItem> Distinct<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TDocument, bool>> filter = null)
        {
            var arrayPropertyString = arrayProperty.Body.ToString();
            arrayPropertyString = arrayPropertyString.Substring(arrayPropertyString.IndexOf('.') + 1);
            var result = _collection.Value.Distinct<TItem>(arrayPropertyString, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TItem>(result);
        }

        public async Task<IDocumentCursor<TItem>> DistinctAsync<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TDocument, bool>> filter = null)
        {
            var arrayPropertyString = arrayProperty.Body.ToString();
            arrayPropertyString = arrayPropertyString.Substring(arrayPropertyString.IndexOf('.') + 1);
            var result = await _collection.Value.DistinctAsync<TItem>(arrayPropertyString, _filterBuilder.CreateMongoFilter(filter));
            return new MongoDocumentCursor<TItem>(result);
        }


        public IDocumentFindCursor<TDocument, TDocument> Find(Expression<Func<TDocument, bool>> filter = null)
        {
            var findCursor = MongoDocumentFindCursor<TDocument, TDocument>.Create(_collection.Value, _filterBuilder);

            findCursor.Where(filter);

            return findCursor;
        }

        public IDocumentFindCursor<TDocument, TDocument> FindText(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false, Expression<Func<TDocument, bool>> filter = null)
        {
            var findCursor = MongoDocumentFindCursor<TDocument, TDocument>.Create(_collection.Value, _filterBuilder);

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


        public IDocumentAggregateCursor<TDocument> Aggregate(Expression<Func<TDocument, bool>> filter = null)
        {
            var aggregateCursor = _collection.Value.Aggregate();

            if (filter != null)
            {
                aggregateCursor = aggregateCursor.Match(_filterBuilder.CreateMongoFilter(filter));
            }

            return new MongoDocumentAggregateCursor<TDocument>(_database, aggregateCursor);
        }


        public void InsertOne(TDocument document)
        {
            _collection.Value.InsertOne(document);
        }

        public Task InsertOneAsync(TDocument document)
        {
            return _collection.Value.InsertOneAsync(document);
        }

        public void InsertMany(IEnumerable<TDocument> documents)
        {
            _collection.Value.InsertMany(documents);
        }

        public Task InsertManyAsync(IEnumerable<TDocument> documents)
        {
            return _collection.Value.InsertManyAsync(documents);
        }


        public DocumentUpdateResult UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateOne(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateOneAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateOneAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public DocumentUpdateResult UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.UpdateMany(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> UpdateManyAsync(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.UpdateManyAsync(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update), new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateUpdateResult(result, insertIfNotExists);
        }


        public DocumentUpdateResult ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = _collection.Value.ReplaceOne(_filterBuilder.CreateMongoFilter(filter), replacement, new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateReplaceResult(result, insertIfNotExists);
        }

        public async Task<DocumentUpdateResult> ReplaceOneAsync(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            var result = await _collection.Value.ReplaceOneAsync(_filterBuilder.CreateMongoFilter(filter), replacement, new UpdateOptions { IsUpsert = insertIfNotExists });
            return MongoHelpers.CreateReplaceResult(result, insertIfNotExists);
        }


        public long DeleteOne(Expression<Func<TDocument, bool>> filter = null)
        {
            var result = _collection.Value.DeleteOne(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public async Task<long> DeleteOneAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            var result = await _collection.Value.DeleteOneAsync(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public long DeleteMany(Expression<Func<TDocument, bool>> filter = null)
        {
            var result = _collection.Value.DeleteMany(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }

        public async Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> filter = null)
        {
            var result = await _collection.Value.DeleteManyAsync(_filterBuilder.CreateMongoFilter(filter));
            return (result.IsAcknowledged) ? result.DeletedCount : 0;
        }


        public DocumentBulkResult Bulk(Action<IDocumentBulkBuilder<TDocument>> requests, bool isOrdered = false)
        {
            var bulk = MongoDocumentBulkBuilderGeneric<TDocument>.CreateMongoBulk(_filterBuilder, requests);
            var result = _collection.Value.BulkWrite(bulk, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }

        public async Task<DocumentBulkResult> BulkAsync(Action<IDocumentBulkBuilder<TDocument>> requests, bool isOrdered = false)
        {
            var bulk = MongoDocumentBulkBuilderGeneric<TDocument>.CreateMongoBulk(_filterBuilder, requests);
            var result = await _collection.Value.BulkWriteAsync(bulk, new BulkWriteOptions { IsOrdered = isOrdered });
            return new DocumentBulkResult(result.RequestCount, result.MatchedCount, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
        }
    }
}
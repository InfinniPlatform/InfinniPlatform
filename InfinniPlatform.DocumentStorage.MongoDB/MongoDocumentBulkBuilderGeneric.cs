using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Abstractions;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы создания набора операций изменения документов в MongoDB.
    /// </summary>
    internal class MongoDocumentBulkBuilderGeneric<TDocument> : IDocumentBulkBuilder<TDocument>
    {
        private MongoDocumentBulkBuilderGeneric(MongoDocumentFilterBuilder<TDocument> filterBuilder = null)
        {
            _filterBuilder = filterBuilder;
        }


        private readonly MongoDocumentFilterBuilder<TDocument> _filterBuilder;
        private readonly List<WriteModel<TDocument>> _operations = new List<WriteModel<TDocument>>();


        public IDocumentBulkBuilder<TDocument> InsertOne(TDocument document)
        {
            _operations.Add(new InsertOneModel<TDocument>(document));
            return this;
        }

        public IDocumentBulkBuilder<TDocument> UpdateOne(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateOneModel<TDocument>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder<TDocument> UpdateMany(Action<IDocumentUpdateBuilder<TDocument>> update, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateManyModel<TDocument>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<TDocument>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder<TDocument> ReplaceOne(TDocument replacement, Expression<Func<TDocument, bool>> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new ReplaceOneModel<TDocument>(_filterBuilder.CreateMongoFilter(filter), replacement) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder<TDocument> DeleteOne(Expression<Func<TDocument, bool>> filter = null)
        {
            _operations.Add(new DeleteOneModel<TDocument>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }


        public IDocumentBulkBuilder<TDocument> DeleteMany(Expression<Func<TDocument, bool>> filter = null)
        {
            _operations.Add(new DeleteManyModel<TDocument>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }


        public static IEnumerable<WriteModel<TDocument>> CreateMongoBulk(MongoDocumentFilterBuilder<TDocument> filterBuilder, Action<IDocumentBulkBuilder<TDocument>> requests)
        {
            var builder = new MongoDocumentBulkBuilderGeneric<TDocument>(filterBuilder);

            requests?.Invoke(builder);

            return builder._operations;
        }
    }
}
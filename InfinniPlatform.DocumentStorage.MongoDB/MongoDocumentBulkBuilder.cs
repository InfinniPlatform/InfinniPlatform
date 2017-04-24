using System;
using System.Collections.Generic;

using InfinniPlatform.Dynamic;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет методы создания набора операций изменения документов в MongoDB.
    /// </summary>
    internal class MongoDocumentBulkBuilder : IDocumentBulkBuilder
    {
        private MongoDocumentBulkBuilder(MongoDocumentFilterBuilder<DynamicDocument> filterBuilder = null)
        {
            _filterBuilder = filterBuilder;
        }


        private readonly MongoDocumentFilterBuilder<DynamicDocument> _filterBuilder;
        private readonly List<WriteModel<DynamicDocument>> _operations = new List<WriteModel<DynamicDocument>>();


        public IDocumentBulkBuilder InsertOne(DynamicDocument document)
        {
            _operations.Add(new InsertOneModel<DynamicDocument>(document));
            return this;
        }

        public IDocumentBulkBuilder UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateOneModel<DynamicDocument>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateManyModel<DynamicDocument>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicDocument>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder ReplaceOne(DynamicDocument replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new ReplaceOneModel<DynamicDocument>(_filterBuilder.CreateMongoFilter(filter), replacement) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder DeleteOne(Func<IDocumentFilterBuilder, object> filter = null)
        {
            _operations.Add(new DeleteOneModel<DynamicDocument>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }

        public IDocumentBulkBuilder DeleteMany(Func<IDocumentFilterBuilder, object> filter = null)
        {
            _operations.Add(new DeleteManyModel<DynamicDocument>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }


        public static IEnumerable<WriteModel<DynamicDocument>> CreateMongoBulk(MongoDocumentFilterBuilder<DynamicDocument> filterBuilder, Action<IDocumentBulkBuilder> requests)
        {
            var builder = new MongoDocumentBulkBuilder(filterBuilder);

            requests?.Invoke(builder);

            return builder._operations;
        }
    }
}
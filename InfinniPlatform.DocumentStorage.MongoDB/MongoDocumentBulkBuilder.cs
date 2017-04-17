using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.DocumentStorage.Abstractions;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы создания набора операций изменения документов в MongoDB.
    /// </summary>
    internal class MongoDocumentBulkBuilder : IDocumentBulkBuilder
    {
        private MongoDocumentBulkBuilder(MongoDocumentFilterBuilder<DynamicWrapper> filterBuilder = null)
        {
            _filterBuilder = filterBuilder;
        }


        private readonly MongoDocumentFilterBuilder<DynamicWrapper> _filterBuilder;
        private readonly List<WriteModel<DynamicWrapper>> _operations = new List<WriteModel<DynamicWrapper>>();


        public IDocumentBulkBuilder InsertOne(DynamicWrapper document)
        {
            _operations.Add(new InsertOneModel<DynamicWrapper>(document));
            return this;
        }

        public IDocumentBulkBuilder UpdateOne(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateOneModel<DynamicWrapper>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder UpdateMany(Action<IDocumentUpdateBuilder> update, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new UpdateManyModel<DynamicWrapper>(_filterBuilder.CreateMongoFilter(filter), MongoDocumentUpdateBuilder<DynamicWrapper>.CreateMongoUpdate(update)) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder ReplaceOne(DynamicWrapper replacement, Func<IDocumentFilterBuilder, object> filter = null, bool insertIfNotExists = false)
        {
            _operations.Add(new ReplaceOneModel<DynamicWrapper>(_filterBuilder.CreateMongoFilter(filter), replacement) { IsUpsert = insertIfNotExists });
            return this;
        }

        public IDocumentBulkBuilder DeleteOne(Func<IDocumentFilterBuilder, object> filter = null)
        {
            _operations.Add(new DeleteOneModel<DynamicWrapper>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }

        public IDocumentBulkBuilder DeleteMany(Func<IDocumentFilterBuilder, object> filter = null)
        {
            _operations.Add(new DeleteManyModel<DynamicWrapper>(_filterBuilder.CreateMongoFilter(filter)));
            return this;
        }


        public static IEnumerable<WriteModel<DynamicWrapper>> CreateMongoBulk(MongoDocumentFilterBuilder<DynamicWrapper> filterBuilder, Action<IDocumentBulkBuilder> requests)
        {
            var builder = new MongoDocumentBulkBuilder(filterBuilder);

            requests?.Invoke(builder);

            return builder._operations;
        }
    }
}
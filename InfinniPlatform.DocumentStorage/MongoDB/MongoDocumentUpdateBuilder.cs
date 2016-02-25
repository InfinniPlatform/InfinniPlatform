using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы создания операторов обновления документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentUpdateBuilder<TDocument> : IDocumentUpdateBuilder<TDocument>, IDocumentUpdateBuilder
    {
        private static readonly UpdateDefinitionBuilder<TDocument> InternalBuilder = Builders<TDocument>.Update;


        private MongoDocumentUpdateBuilder()
        {
        }


        private readonly List<UpdateDefinition<TDocument>> _updates = new List<UpdateDefinition<TDocument>>();


        public IDocumentUpdateBuilder<TDocument> Rename(Expression<Func<TDocument, object>> property, string newProperty)
        {
            _updates.Add(InternalBuilder.Rename(property, newProperty));
            return this;
        }

        public IDocumentUpdateBuilder Rename(string property, string newProperty)
        {
            _updates.Add(InternalBuilder.Rename(property, newProperty));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Remove(Expression<Func<TDocument, object>> property)
        {
            _updates.Add(InternalBuilder.Unset(property));
            return this;
        }

        public IDocumentUpdateBuilder Remove(string property)
        {
            _updates.Add(InternalBuilder.Unset(property));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Set<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.Set(property, value));
            return this;
        }

        public IDocumentUpdateBuilder Set<TProperty>(string property, TProperty value)
        {
            _updates.Add(InternalBuilder.Set(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Inc<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.Inc(property, value));
            return this;
        }

        public IDocumentUpdateBuilder Inc(string property, object value)
        {
            _updates.Add(InternalBuilder.Inc(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Mul<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.Mul(property, value));
            return this;
        }

        public IDocumentUpdateBuilder Mul(string property, object value)
        {
            _updates.Add(InternalBuilder.Mul(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Min<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.Min(property, value));
            return this;
        }

        public IDocumentUpdateBuilder Min(string property, object value)
        {
            _updates.Add(InternalBuilder.Min(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Max<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.Max(property, value));
            return this;
        }

        public IDocumentUpdateBuilder Max(string property, object value)
        {
            _updates.Add(InternalBuilder.Max(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> BitwiseOr<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.BitwiseOr(property, value));
            return this;
        }

        public IDocumentUpdateBuilder BitwiseOr(string property, object value)
        {
            _updates.Add(InternalBuilder.BitwiseOr(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> BitwiseAnd<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.BitwiseAnd(property, value));
            return this;
        }

        public IDocumentUpdateBuilder BitwiseAnd(string property, object value)
        {
            _updates.Add(InternalBuilder.BitwiseAnd(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> BitwiseXor<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value)
        {
            _updates.Add(InternalBuilder.BitwiseXor(property, value));
            return this;
        }

        public IDocumentUpdateBuilder BitwiseXor(string property, object value)
        {
            _updates.Add(InternalBuilder.BitwiseXor(property, value));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> CurrentDate(Expression<Func<TDocument, object>> property)
        {
            _updates.Add(InternalBuilder.CurrentDate(property));
            return this;
        }

        public IDocumentUpdateBuilder CurrentDate(string property)
        {
            _updates.Add(InternalBuilder.CurrentDate(property));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Push<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item)
        {
            _updates.Add(InternalBuilder.Push(arrayProperty, item));
            return this;
        }

        public IDocumentUpdateBuilder Push(string arrayProperty, object item)
        {
            _updates.Add(InternalBuilder.Push(arrayProperty, item));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PushAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.PushEach(arrayProperty, items));
            return this;
        }

        public IDocumentUpdateBuilder PushAll<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.PushEach(arrayProperty, items));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PushUnique<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item)
        {
            _updates.Add(InternalBuilder.AddToSet(arrayProperty, item));
            return this;
        }

        public IDocumentUpdateBuilder PushUnique(string arrayProperty, object item)
        {
            _updates.Add(InternalBuilder.AddToSet(arrayProperty, item));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PushAllUnique<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.AddToSetEach(arrayProperty, items));
            return this;
        }

        public IDocumentUpdateBuilder PushAllUnique<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.AddToSetEach(arrayProperty, items));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PopFirst(Expression<Func<TDocument, object>> arrayProperty)
        {
            _updates.Add(InternalBuilder.PopFirst(arrayProperty));
            return this;
        }

        public IDocumentUpdateBuilder PopFirst(string arrayProperty)
        {
            _updates.Add(InternalBuilder.PopFirst(arrayProperty));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PopLast(Expression<Func<TDocument, object>> arrayProperty)
        {
            _updates.Add(InternalBuilder.PopLast(arrayProperty));
            return this;
        }

        public IDocumentUpdateBuilder PopLast(string arrayProperty)
        {
            _updates.Add(InternalBuilder.PopLast(arrayProperty));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> Pull<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item)
        {
            _updates.Add(InternalBuilder.Pull(arrayProperty, item));
            return this;
        }

        public IDocumentUpdateBuilder Pull(string arrayProperty, object item)
        {
            _updates.Add(InternalBuilder.Pull(arrayProperty, item));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PullAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.PullAll(arrayProperty, items));
            return this;
        }

        public IDocumentUpdateBuilder PullAll<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            _updates.Add(InternalBuilder.PullAll(arrayProperty, items));
            return this;
        }


        public IDocumentUpdateBuilder<TDocument> PullFilter<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TItem, bool>> filter = null)
        {
            var filterBuilder = new MongoDocumentFilterBuilder<TItem>();
            _updates.Add(InternalBuilder.PullFilter(arrayProperty, filterBuilder.CreateMongoFilter(filter)));
            return this;
        }

        public IDocumentUpdateBuilder PullFilter(string arrayProperty, Func<IDocumentFilterBuilder, object> filter = null)
        {
            var filterBuilder = new MongoDocumentFilterBuilder<TDocument>();
            _updates.Add(InternalBuilder.PullFilter(arrayProperty, filterBuilder.CreateMongoFilter(filter)));
            return this;
        }


        public static UpdateDefinition<TDocument> CreateMongoUpdate(Action<IDocumentUpdateBuilder> update)
        {
            var builder = new MongoDocumentUpdateBuilder<TDocument>();

            update?.Invoke(builder);

            return (builder._updates.Count != 1) ? InternalBuilder.Combine(builder._updates) : builder._updates[0];
        }

        public static UpdateDefinition<TDocument> CreateMongoUpdate(Action<IDocumentUpdateBuilder<TDocument>> update)
        {
            var builder = new MongoDocumentUpdateBuilder<TDocument>();

            update?.Invoke(builder);

            return (builder._updates.Count != 1) ? InternalBuilder.Combine(builder._updates) : builder._updates[0];
        }
    }
}
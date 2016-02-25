using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы создания проекции данных документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentProjectionBuilder<TDocument> : IDocumentProjectionBuilder
    {
        private static readonly ProjectionDefinitionBuilder<TDocument> InternalBuilder = Builders<TDocument>.Projection;


        private MongoDocumentProjectionBuilder()
        {
        }


        private readonly List<ProjectionDefinition<TDocument>> _projections = new List<ProjectionDefinition<TDocument>>();


        public IDocumentProjectionBuilder Include(string property)
        {
            _projections.Add(InternalBuilder.Include(property));
            return this;
        }

        public IDocumentProjectionBuilder Exclude(string property)
        {
            _projections.Add(InternalBuilder.Exclude(property));
            return this;
        }


        public IDocumentProjectionBuilder Match(string arrayProperty, Func<IDocumentFilterBuilder, object> filter = null)
        {
            var filterBuilder = new MongoDocumentFilterBuilder<TDocument>();
            _projections.Add(InternalBuilder.ElemMatch(arrayProperty, filterBuilder.CreateMongoFilter(filter)));
            return this;
        }

        public IDocumentProjectionBuilder Slice(string arrayProperty, int count)
        {
            _projections.Add(InternalBuilder.Slice(arrayProperty, count));
            return this;
        }

        public IDocumentProjectionBuilder Slice(string arrayProperty, int index, int limit)
        {
            _projections.Add(InternalBuilder.Slice(arrayProperty, index, limit));
            return this;
        }


        public IDocumentProjectionBuilder IncludeTextScore(string property)
        {
            _projections.Add(InternalBuilder.MetaTextScore(property));
            return this;
        }


        public static ProjectionDefinition<TDocument> CreateMongoProjection(Action<IDocumentProjectionBuilder> projection)
        {
            var builder = new MongoDocumentProjectionBuilder<TDocument>();

            projection?.Invoke(builder);

            return (builder._projections.Count != 1) ? InternalBuilder.Combine(builder._projections) : builder._projections[0];
        }
    }
}
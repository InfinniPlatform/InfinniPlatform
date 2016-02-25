using System;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Bson;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Указатель на список документов для поиска в MongoDB.
    /// </summary>
    internal class MongoDocumentFindCursor<TDocument, TProjection> : MongoDocumentCursor<TProjection>, IDocumentFindSortedCursor<TDocument, TProjection>
    {
        public MongoDocumentFindCursor(IFindFluent<TDocument, TProjection> fluentCursor, SortDefinition<TDocument> sortDefinitionDefinition = null, string textScoreProperty = null)
        {
            _fluentCursor = fluentCursor;
            _sortDefinition = sortDefinitionDefinition;
            _textScoreProperty = textScoreProperty;
        }


        private readonly IFindFluent<TDocument, TProjection> _fluentCursor;
        private SortDefinition<TDocument> _sortDefinition;
        private string _textScoreProperty;


        protected override IAsyncCursor<TProjection> Cursor => CreateCursor();


        public IDocumentFindCursor<TDocument, TNewProjection> Project<TNewProjection>(Expression<Func<TDocument, TNewProjection>> projection)
        {
            var fluentCursor = _fluentCursor.Project(projection);
            return new MongoDocumentFindCursor<TDocument, TNewProjection>(fluentCursor, _sortDefinition, _textScoreProperty);
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortBy(Expression<Func<TDocument, object>> property)
        {
            _sortDefinition = Builders<TDocument>.Sort.Ascending(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortByDescending(Expression<Func<TDocument, object>> property)
        {
            _sortDefinition = Builders<TDocument>.Sort.Descending(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortByTextScore(Expression<Func<TProjection, object>> textScoreProperty)
        {
            _textScoreProperty = MongoHelpers.GetPropertyName(textScoreProperty);
            _sortDefinition = Builders<TDocument>.Sort.MetaTextScore(_textScoreProperty);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenBy(Expression<Func<TDocument, object>> property)
        {
            var thenSortDefinition = Builders<TDocument>.Sort.Ascending(property);
            _sortDefinition = Builders<TDocument>.Sort.Combine(_sortDefinition, thenSortDefinition);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenByDescending(Expression<Func<TDocument, object>> property)
        {
            var thenSortDefinition = Builders<TDocument>.Sort.Descending(property);
            _sortDefinition = Builders<TDocument>.Sort.Combine(_sortDefinition, thenSortDefinition);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenByTextScore(Expression<Func<TProjection, object>> textScoreProperty)
        {
            _textScoreProperty = MongoHelpers.GetPropertyName(textScoreProperty);
            var thenSortDefinition = Builders<TDocument>.Sort.MetaTextScore(_textScoreProperty);
            _sortDefinition = Builders<TDocument>.Sort.Combine(_sortDefinition, thenSortDefinition);
            return this;
        }

        public IDocumentFindCursor<TDocument, TProjection> Skip(int skip)
        {
            _fluentCursor.Skip(skip);
            return this;
        }

        public IDocumentFindCursor<TDocument, TProjection> Limit(int limit)
        {
            _fluentCursor.Limit(limit);
            return this;
        }


        private IAsyncCursor<TProjection> CreateCursor()
        {
            var fluentCursor = _fluentCursor;

            if (!string.IsNullOrEmpty(_textScoreProperty))
            {
                var projectionDefinition = fluentCursor.Options.Projection;

                var textScoreProjection = new BsonDocument(_textScoreProperty, new BsonDocument("$meta", "textScore"));
                var textScoreProjectionDefinition = new BsonDocumentProjectionDefinition<TDocument, TProjection>(textScoreProjection);

                fluentCursor.Options.Projection = (projectionDefinition != null)
                    ? MongoHelpers.Combine(projectionDefinition, textScoreProjectionDefinition)
                    : textScoreProjectionDefinition;
            }

            if (_sortDefinition != null)
            {
                fluentCursor = _fluentCursor.Sort(_sortDefinition);
            }

            return fluentCursor.ToCursor();
        }
    }
}
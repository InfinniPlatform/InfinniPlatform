using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Abstractions;

using MongoDB.Bson;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Указатель на список документов для поиска в MongoDB.
    /// </summary>
    internal class MongoDocumentFindCursor<TDocument, TProjection> : MongoDocumentCursor<TProjection>, IDocumentFindSortedCursor<TDocument, TProjection>
    {
        public static MongoDocumentFindCursor<TDocument, TDocument> Create(IMongoCollection<TDocument> collection, MongoDocumentFilterBuilder<TDocument> filterBuilder)
        {
            var emptyFilter = filterBuilder.EmptyMongoFilter();
            var filterCursor = collection.Find(emptyFilter);

            return new MongoDocumentFindCursor<TDocument, TDocument>(filterCursor, filterBuilder, null, null);
        }


        private MongoDocumentFindCursor(IFindFluent<TDocument, TProjection> findCursor, MongoDocumentFilterBuilder<TDocument> filterBuilder, SortDefinition<TDocument> sortDefinition, string textScoreProperty)
        {
            _findCursor = findCursor;
            _filterBuilder = filterBuilder;
            _sortDefinition = sortDefinition;
            _textScoreProperty = textScoreProperty;
        }


        private readonly IFindFluent<TDocument, TProjection> _findCursor;
        private readonly MongoDocumentFilterBuilder<TDocument> _filterBuilder;

        private SortDefinition<TDocument> _sortDefinition;
        private string _textScoreProperty;


        protected override IAsyncCursor<TProjection> Cursor => CreateCursor();


        public IDocumentFindCursor<TDocument, TProjection> Where(Expression<Func<TDocument, bool>> filter)
        {
            var filterDefinition = _filterBuilder.CreateMongoFilter(filter);
            _findCursor.Filter = _findCursor.Filter & filterDefinition;
            return this;
        }

        public IDocumentFindCursor<TDocument, TProjection> Where(Func<IDocumentFilterBuilder, object> filter)
        {
            var filterDefinition = _filterBuilder.CreateMongoFilter(filter);
            _findCursor.Filter = _findCursor.Filter & filterDefinition;
            return this;
        }

        public IDocumentFindCursor<TDocument, TNewProjection> Project<TNewProjection>(Expression<Func<TDocument, TNewProjection>> projection)
        {
            var projectCursor = _findCursor.Project(projection);
            return new MongoDocumentFindCursor<TDocument, TNewProjection>(projectCursor, _filterBuilder, _sortDefinition, _textScoreProperty);
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
            _findCursor.Skip(skip);
            return this;
        }

        public IDocumentFindCursor<TDocument, TProjection> Limit(int limit)
        {
            _findCursor.Limit(limit);
            return this;
        }

        public long Count()
        {
            return _findCursor.Count();
        }

        public Task<long> CountAsync()
        {
            return _findCursor.CountAsync();
        }


        private IAsyncCursor<TProjection> CreateCursor()
        {
            var findCursor = _findCursor;

            if (!string.IsNullOrEmpty(_textScoreProperty))
            {
                var projectionDefinition = findCursor.Options.Projection;

                var textScoreProjection = new BsonDocument(_textScoreProperty, new BsonDocument("$meta", "textScore"));
                var textScoreProjectionDefinition = new BsonDocumentProjectionDefinition<TDocument, TProjection>(textScoreProjection);

                findCursor.Options.Projection = (projectionDefinition != null)
                    ? MongoHelpers.Combine(projectionDefinition, textScoreProjectionDefinition)
                    : textScoreProjectionDefinition;
            }

            if (_sortDefinition != null)
            {
                findCursor = _findCursor.Sort(_sortDefinition);
            }

            return findCursor.ToCursor();
        }
    }
}
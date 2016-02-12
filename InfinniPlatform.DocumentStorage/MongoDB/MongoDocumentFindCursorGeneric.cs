using System;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Указатель на список документов для поиска в MongoDB.
    /// </summary>
    internal class MongoDocumentFindCursorGeneric<TDocument, TProjection> : MongoDocumentCursor<TProjection>, IDocumentFindSortedCursor<TDocument, TProjection>
    {
        public MongoDocumentFindCursorGeneric(FindFluentBase<TDocument, TProjection> fluentCursor)
        {
            _fluentCursor = fluentCursor;
        }


        private readonly IFindFluent<TDocument, TProjection> _fluentCursor;
        private SortDefinition<TDocument> _sort;


        protected override IAsyncCursor<TProjection> Cursor => CreateCursor();


        public IDocumentFindCursor<TDocument, TNewProjection> Project<TNewProjection>(Expression<Func<TDocument, TNewProjection>> projection)
        {
            var fluentCursor = (FindFluentBase<TDocument, TNewProjection>)_fluentCursor.Project(projection);
            return new MongoDocumentFindCursorGeneric<TDocument, TNewProjection>(fluentCursor);
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortBy(Expression<Func<TDocument, object>> property)
        {
            _sort = Builders<TDocument>.Sort.Ascending(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortByDescending(Expression<Func<TDocument, object>> property)
        {
            _sort = Builders<TDocument>.Sort.Descending(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortByTextScore(Expression<Func<TProjection, object>> property)
        {
            _sort = Builders<TDocument>.Sort.MetaTextScore(GetPropertyName(property));
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenBy(Expression<Func<TDocument, object>> property)
        {
            var thenSort = Builders<TDocument>.Sort.Ascending(property);
            _sort = Builders<TDocument>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenByDescending(Expression<Func<TDocument, object>> property)
        {
            var thenSort = Builders<TDocument>.Sort.Descending(property);
            _sort = Builders<TDocument>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenByTextScore(Expression<Func<TProjection, object>> property)
        {
            var thenSort = Builders<TDocument>.Sort.MetaTextScore(GetPropertyName(property));
            _sort = Builders<TDocument>.Sort.Combine(_sort, thenSort);
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


        private static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            return ((MemberExpression)property.Body).Member.Name;
        }


        private IAsyncCursor<TProjection> CreateCursor()
        {
            var fluentCursor = _fluentCursor;

            if (_sort != null)
            {
                fluentCursor = _fluentCursor.Sort(_sort);
            }

            return fluentCursor.ToCursor();
        }
    }
}
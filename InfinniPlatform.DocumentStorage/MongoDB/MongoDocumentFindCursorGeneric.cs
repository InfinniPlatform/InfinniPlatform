using System;
using System.Linq.Expressions;
using System.Threading;

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


        private readonly FindFluentBase<TDocument, TProjection> _fluentCursor;


        protected override IAsyncCursor<TProjection> Cursor => CreateCursor();


        public IDocumentFindCursor<TDocument, TNewProjection> Project<TNewProjection>(Expression<Func<TDocument, TNewProjection>> projection)
        {
            var fluentCursor = (FindFluentBase<TDocument, TNewProjection>)_fluentCursor.Project(projection);
            return new MongoDocumentFindCursorGeneric<TDocument, TNewProjection>(fluentCursor);
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortBy(Expression<Func<TDocument, object>> property)
        {
            _fluentCursor.SortBy(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> SortByDescending(Expression<Func<TDocument, object>> property)
        {
            _fluentCursor.SortByDescending(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenBy(Expression<Func<TDocument, object>> property)
        {
            _fluentCursor.ThenBy(property);
            return this;
        }

        public IDocumentFindSortedCursor<TDocument, TProjection> ThenByDescending(Expression<Func<TDocument, object>> property)
        {
            _fluentCursor.ThenByDescending(property);
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
            return _fluentCursor.ToCursor(default(CancellationToken));
        }
    }
}
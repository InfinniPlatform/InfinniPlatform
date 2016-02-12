using System;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Указатель на список документов MongoDB.
    /// </summary>
    internal sealed class MongoDocumentFindCursor : MongoDocumentCursor<DynamicWrapper>, IDocumentFindSortedCursor
    {
        public MongoDocumentFindCursor(Lazy<IMongoCollection<DynamicWrapper>> collection, FilterDefinition<DynamicWrapper> filter)
        {
            _collection = collection;
            _filter = filter;
        }


        private readonly Lazy<IMongoCollection<DynamicWrapper>> _collection;
        private readonly FilterDefinition<DynamicWrapper> _filter;
        private ProjectionDefinition<DynamicWrapper> _projection;
        private SortDefinition<DynamicWrapper> _sort;
        private int? _skip;
        private int? _limit;


        protected override IAsyncCursor<DynamicWrapper> Cursor => CreateCursor();


        public IDocumentFindCursor Project(Action<IDocumentProjectionBuilder> projection)
        {
            _projection = MongoDocumentProjectionBuilder<DynamicWrapper>.CreateMongoProjection(projection);
            return this;
        }

        public IDocumentFindSortedCursor SortBy(string property)
        {
            _sort = Builders<DynamicWrapper>.Sort.Ascending(property);
            return this;
        }

        public IDocumentFindSortedCursor SortByDescending(string property)
        {
            _sort = Builders<DynamicWrapper>.Sort.Descending(property);
            return this;
        }

        public IDocumentFindSortedCursor SortByTextScore(string property)
        {
            _sort = Builders<DynamicWrapper>.Sort.MetaTextScore(property);
            return this;
        }

        public IDocumentFindSortedCursor ThenBy(string property)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.Ascending(property);
            _sort = Builders<DynamicWrapper>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentFindSortedCursor ThenByDescending(string property)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.Descending(property);
            _sort = Builders<DynamicWrapper>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentFindSortedCursor ThenByTextScore(string property)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.MetaTextScore(property);
            _sort = Builders<DynamicWrapper>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentFindCursor Skip(int skip)
        {
            _skip = skip;
            return this;
        }

        public IDocumentFindCursor Limit(int limit)
        {
            _limit = limit;
            return this;
        }


        private IAsyncCursor<DynamicWrapper> CreateCursor()
        {
            var findOptions = new FindOptions<DynamicWrapper, DynamicWrapper> { Skip = _skip, Limit = _limit };

            if (_projection != null)
            {
                findOptions.Projection = _projection;
            }

            if (_sort != null)
            {
                findOptions.Sort = _sort;
            }

            return _collection.Value.FindSync(_filter, findOptions);
        }
    }
}
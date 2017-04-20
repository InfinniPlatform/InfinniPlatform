using System;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Указатель на список документов MongoDB.
    /// </summary>
    internal class MongoDocumentFindCursor : MongoDocumentCursor<DynamicWrapper>, IDocumentFindSortedCursor
    {
        public MongoDocumentFindCursor(Lazy<IMongoCollection<DynamicWrapper>> collection, MongoDocumentFilterBuilder<DynamicWrapper> filterBuilder)
        {
            _collection = collection;
            _filterBuilder = filterBuilder;
        }


        private readonly Lazy<IMongoCollection<DynamicWrapper>> _collection;
        private readonly MongoDocumentFilterBuilder<DynamicWrapper> _filterBuilder;

        private FilterDefinition<DynamicWrapper> _filter;
        private ProjectionDefinition<DynamicWrapper> _projection;
        private SortDefinition<DynamicWrapper> _sort;
        private int? _skip;
        private int? _limit;


        protected override IAsyncCursor<DynamicWrapper> Cursor => CreateCursor();


        public IDocumentFindCursor Where(Func<IDocumentFilterBuilder, object> filter)
        {
            var filterDefinition = _filterBuilder.CreateMongoFilter(filter);
            _filter = (_filter != null) ? _filter & filterDefinition : filterDefinition;
            return this;
        }

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

        public IDocumentFindSortedCursor SortByTextScore(string textScoreProperty = DocumentStorageExtensions.DefaultTextScoreProperty)
        {
            _sort = Builders<DynamicWrapper>.Sort.MetaTextScore(textScoreProperty);
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

        public IDocumentFindSortedCursor ThenByTextScore(string textScoreProperty = DocumentStorageExtensions.DefaultTextScoreProperty)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.MetaTextScore(textScoreProperty);
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

        public long Count()
        {
            return _collection.Value.Count(_filter);
        }

        public Task<long> CountAsync()
        {
            return _collection.Value.CountAsync(_filter);
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

            return _collection.Value.FindSync(_filter ?? _filterBuilder.EmptyMongoFilter(), findOptions);
        }
    }
}
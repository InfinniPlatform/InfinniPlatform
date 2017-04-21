using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Dynamic;

using MongoDB.Bson;
using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Указатель на список документов для агрегации MongoDB.
    /// </summary>
    internal class MongoDocumentAggregateCursor : MongoDocumentCursor<DynamicWrapper>, IDocumentAggregateSortedCursor
    {
        public MongoDocumentAggregateCursor(IAggregateFluent<DynamicWrapper> aggregateCursor)
        {
            _aggregateCursor = aggregateCursor;
        }


        private readonly IAggregateFluent<DynamicWrapper> _aggregateCursor;

        private readonly List<Func<IAggregateFluent<DynamicWrapper>, IAggregateFluent<DynamicWrapper>>> _stages
            = new List<Func<IAggregateFluent<DynamicWrapper>, IAggregateFluent<DynamicWrapper>>>();

        private SortDefinition<DynamicWrapper> _sort;


        protected override IAsyncCursor<DynamicWrapper> Cursor => CreateCursor();


        public IDocumentAggregateCursor Project(DynamicWrapper projection)
        {
            _stages.Add(f => f.Project(ToProjectionDefinition(projection)));
            return this;
        }

        public IDocumentAggregateCursor Unwind(string arrayProperty)
        {
            _stages.Add(f => f.Unwind<DynamicWrapper>(arrayProperty));
            return this;
        }

        public IDocumentAggregateCursor Group(DynamicWrapper group)
        {
            _stages.Add(f => f.Group(ToProjectionDefinition(group)));
            return this;
        }

        public IDocumentAggregateCursor Lookup(string foreignDocumentType, string localKeyProperty, string foreignKeyProperty, string resultArrayProperty)
        {
            _stages.Add(f => f.Lookup<DynamicWrapper, DynamicWrapper>(foreignKeyProperty, localKeyProperty, foreignKeyProperty, resultArrayProperty));
            return this;
        }

        public IDocumentAggregateSortedCursor SortBy(string property)
        {
            _sort = Builders<DynamicWrapper>.Sort.Ascending(property);
            return this;
        }

        public IDocumentAggregateSortedCursor SortByDescending(string property)
        {
            _sort = Builders<DynamicWrapper>.Sort.Descending(property);
            return this;
        }

        public IDocumentAggregateSortedCursor ThenBy(string property)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.Ascending(property);
            _sort = Builders<DynamicWrapper>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentAggregateSortedCursor ThenByDescending(string property)
        {
            var thenSort = Builders<DynamicWrapper>.Sort.Ascending(property);
            _sort = Builders<DynamicWrapper>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentAggregateCursor Skip(int skip)
        {
            _stages.Add(f => f.Skip(skip));
            return this;
        }

        public IDocumentAggregateCursor Limit(int limit)
        {
            _stages.Add(f => f.Skip(limit));
            return this;
        }


        private static ProjectionDefinition<DynamicWrapper, DynamicWrapper> ToProjectionDefinition(object projection)
        {
            return ((DynamicWrapper)projection).ToBsonDocument();
        }


        private IAsyncCursor<DynamicWrapper> CreateCursor()
        {
            var aggregateCursor = _stages.Aggregate(_aggregateCursor, (current, stage) => stage(current));

            if (_sort != null)
            {
                aggregateCursor = aggregateCursor.Sort(_sort);
            }

            return aggregateCursor.ToCursor();
        }
    }
}
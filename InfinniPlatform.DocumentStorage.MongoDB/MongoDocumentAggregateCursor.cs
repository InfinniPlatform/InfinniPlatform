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
    internal class MongoDocumentAggregateCursor : MongoDocumentCursor<DynamicDocument>, IDocumentAggregateSortedCursor
    {
        public MongoDocumentAggregateCursor(IAggregateFluent<DynamicDocument> aggregateCursor)
        {
            _aggregateCursor = aggregateCursor;
        }


        private readonly IAggregateFluent<DynamicDocument> _aggregateCursor;

        private readonly List<Func<IAggregateFluent<DynamicDocument>, IAggregateFluent<DynamicDocument>>> _stages
            = new List<Func<IAggregateFluent<DynamicDocument>, IAggregateFluent<DynamicDocument>>>();

        private SortDefinition<DynamicDocument> _sort;


        protected override IAsyncCursor<DynamicDocument> Cursor => CreateCursor();


        public IDocumentAggregateCursor Project(DynamicDocument projection)
        {
            _stages.Add(f => f.Project(ToProjectionDefinition(projection)));
            return this;
        }

        public IDocumentAggregateCursor Unwind(string arrayProperty)
        {
            _stages.Add(f => f.Unwind<DynamicDocument>(arrayProperty));
            return this;
        }

        public IDocumentAggregateCursor Group(DynamicDocument group)
        {
            _stages.Add(f => f.Group(ToProjectionDefinition(group)));
            return this;
        }

        public IDocumentAggregateCursor Lookup(string foreignDocumentType, string localKeyProperty, string foreignKeyProperty, string resultArrayProperty)
        {
            _stages.Add(f => f.Lookup<DynamicDocument, DynamicDocument>(foreignKeyProperty, localKeyProperty, foreignKeyProperty, resultArrayProperty));
            return this;
        }

        public IDocumentAggregateSortedCursor SortBy(string property)
        {
            _sort = Builders<DynamicDocument>.Sort.Ascending(property);
            return this;
        }

        public IDocumentAggregateSortedCursor SortByDescending(string property)
        {
            _sort = Builders<DynamicDocument>.Sort.Descending(property);
            return this;
        }

        public IDocumentAggregateSortedCursor ThenBy(string property)
        {
            var thenSort = Builders<DynamicDocument>.Sort.Ascending(property);
            _sort = Builders<DynamicDocument>.Sort.Combine(_sort, thenSort);
            return this;
        }

        public IDocumentAggregateSortedCursor ThenByDescending(string property)
        {
            var thenSort = Builders<DynamicDocument>.Sort.Ascending(property);
            _sort = Builders<DynamicDocument>.Sort.Combine(_sort, thenSort);
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


        private static ProjectionDefinition<DynamicDocument, DynamicDocument> ToProjectionDefinition(object projection)
        {
            return ((DynamicDocument)projection).ToBsonDocument();
        }


        private IAsyncCursor<DynamicDocument> CreateCursor()
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
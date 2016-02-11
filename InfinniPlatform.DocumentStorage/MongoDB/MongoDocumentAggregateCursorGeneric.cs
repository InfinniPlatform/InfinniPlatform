using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;

using MongoDB.Driver;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal sealed class MongoDocumentAggregateCursor<TResult> : MongoDocumentCursor<TResult>, IDocumentAggregateSortedCursor<TResult>
    {
        public MongoDocumentAggregateCursor(Lazy<IMongoDatabase> database, IAggregateFluent<TResult> fluentAggregateCursor)
        {
            _database = database;
            _fluentAggregateCursor = fluentAggregateCursor;
        }


        private readonly Lazy<IMongoDatabase> _database;
        private readonly IAggregateFluent<TResult> _fluentAggregateCursor;


        protected override IAsyncCursor<TResult> Cursor => CreateCursor();


        private MongoDocumentAggregateCursor<TNewResult> CreateNextStage<TNewResult>(IAggregateFluent<TNewResult> next)
        {
            return new MongoDocumentAggregateCursor<TNewResult>(_database, next);
        }


        public IDocumentAggregateCursor<TNewResult> Project<TNewResult>(Expression<Func<TResult, TNewResult>> projection)
        {
            var next = _fluentAggregateCursor.Project(projection);
            return CreateNextStage(next);
        } 

        public IDocumentAggregateCursor<TNewResult> Unwind<TNewResult>(Expression<Func<TResult, object>> arrayProperty)
        {
            var next = _fluentAggregateCursor.Unwind<TResult, TNewResult>(arrayProperty);
            return CreateNextStage(next);
        }

        public IDocumentAggregateCursor<TNewResult> Group<TKey, TNewResult>(Expression<Func<TResult, TKey>> groupKey, Expression<Func<IGrouping<TKey, TResult>, TNewResult>> groupValue)
        {
            var next = _fluentAggregateCursor.Group(groupKey, groupValue);
            return CreateNextStage(next);
        }

        public IDocumentAggregateCursor<TNewResult> Lookup<TForeignDocument, TNewResult>(string foreignDocumentType, Expression<Func<TResult, object>> localKeyProperty, Expression<Func<TForeignDocument, object>> foreignKeyProperty, Expression<Func<TNewResult, object>> resultArrayProperty)
        {
            var next = _fluentAggregateCursor.Lookup(_database.Value.GetCollection<TForeignDocument>(foreignDocumentType), localKeyProperty, foreignKeyProperty, resultArrayProperty);
            return CreateNextStage(next);
        }

        public IDocumentAggregateSortedCursor<TResult> SortBy(Expression<Func<TResult, object>> property)
        {
            var next = _fluentAggregateCursor.SortBy(property);
            return CreateNextStage(next);
        }

        public IDocumentAggregateSortedCursor<TResult> SortByDescending(Expression<Func<TResult, object>> property)
        {
            var next = _fluentAggregateCursor.SortByDescending(property);
            return CreateNextStage(next);
        }

        public IDocumentAggregateSortedCursor<TResult> ThenBy(Expression<Func<TResult, object>> property)
        {
            var next = ((AggregateFluentBase<TResult>)_fluentAggregateCursor).ThenBy(property);
            return CreateNextStage(next);
        }

        public IDocumentAggregateSortedCursor<TResult> ThenByDescending(Expression<Func<TResult, object>> property)
        {
            var next = ((AggregateFluentBase<TResult>)_fluentAggregateCursor).ThenByDescending(property);
            return CreateNextStage(next);
        }

        public IDocumentAggregateCursor<TResult> Skip(int skip)
        {
            var next = _fluentAggregateCursor.Skip(skip);
            return CreateNextStage(next);
        }

        public IDocumentAggregateCursor<TResult> Limit(int limit)
        {
            var next = _fluentAggregateCursor.Skip(limit);
            return CreateNextStage(next);
        }


        private IAsyncCursor<TResult> CreateCursor()
        {
            return _fluentAggregateCursor.ToCursor();
        }
    }
}
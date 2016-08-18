using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Metadata.Documents;

using MongoDB.Bson;
using MongoDB.Driver;

using RegexClass = System.Text.RegularExpressions.Regex;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет методы создания фильтров для поиска документов в MongoDB.
    /// </summary>
    internal sealed class MongoDocumentFilterBuilder<TDocument> : IDocumentFilterBuilder
    {
        private static readonly FilterDefinitionBuilder<TDocument> InternalBuilder = Builders<TDocument>.Filter;


        public object Empty()
        {
            return EmptyMongoFilter();
        }


        public object Not(object filter)
        {
            return InternalBuilder.Not(AsMongoFilter(filter));
        }


        public object Or(params object[] filters)
        {
            return InternalBuilder.Or(AsMongoFilters(filters));
        }

        public object Or(IEnumerable<object> filters)
        {
            return InternalBuilder.Or(AsMongoFilters(filters));
        }


        public object And(params object[] filters)
        {
            return InternalBuilder.And(AsMongoFilters(filters));
        }

        public object And(IEnumerable<object> filters)
        {
            return InternalBuilder.And(AsMongoFilters(filters));
        }


        public object Exists(string property, bool exists = true)
        {
            return InternalBuilder.Exists(property, exists);
        }

        public object Type(string property, DataType valueType)
        {
            return InternalBuilder.Type(property, BsonTypeMapping[valueType]);
        }


        public object In<TProperty>(string property, IEnumerable<TProperty> values)
        {
            return InternalBuilder.In(property, values);
        }

        public object NotIn<TProperty>(string property, IEnumerable<TProperty> values)
        {
            return InternalBuilder.Nin(property, values);
        }

        public object Eq<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Eq(property, value);
        }

        public object NotEq<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Ne(property, value);
        }

        public object Gt<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Gt(property, value);
        }

        public object Gte<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Gte(property, value);
        }

        public object Lt<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Lt(property, value);
        }

        public object Lte<TProperty>(string property, TProperty value)
        {
            return InternalBuilder.Lte(property, value);
        }

        public object Regex(string property, Regex value)
        {
            return InternalBuilder.Regex(property, value);
        }

        public object StartsWith(string property, string value, bool ignoreCase = true)
        {
            return InternalBuilder.Regex(property, new BsonRegularExpression($"^{RegexClass.Escape(value)}", ignoreCase ? "i" : null));
        }

        public object EndsWith(string property, string value, bool ignoreCase = true)
        {
            return InternalBuilder.Regex(property, new BsonRegularExpression($"{RegexClass.Escape(value)}$", ignoreCase ? "i" : null));
        }

        public object Contains(string property, string value, bool ignoreCase = true)
        {
            return InternalBuilder.Regex(property, new BsonRegularExpression(RegexClass.Escape(value), ignoreCase ? "i" : null));
        }


        public object Match(string arrayProperty, object filter)
        {
            return InternalBuilder.ElemMatch(arrayProperty, AsMongoFilter(filter));
        }

        public object All<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            return InternalBuilder.All(arrayProperty, items);
        }

        public object AnyIn<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            return InternalBuilder.AnyIn(arrayProperty, items);
        }

        public object AnyNotIn<TItem>(string arrayProperty, IEnumerable<TItem> items)
        {
            return InternalBuilder.AnyNin(arrayProperty, items);
        }

        public object AnyEq<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyEq(arrayProperty, item);
        }

        public object AnyNotEq<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyNe(arrayProperty, item);
        }

        public object AnyGt<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyGt(arrayProperty, item);
        }

        public object AnyGte<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyGte(arrayProperty, item);
        }

        public object AnyLt<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyLt(arrayProperty, item);
        }

        public object AnyLte<TItem>(string arrayProperty, TItem item)
        {
            return InternalBuilder.AnyLte(arrayProperty, item);
        }


        public object SizeEq(string arrayProperty, int value)
        {
            return InternalBuilder.Size(arrayProperty, value);
        }

        public object SizeGt(string arrayProperty, int value)
        {
            return InternalBuilder.SizeGt(arrayProperty, value);
        }

        public object SizeGte(string arrayProperty, int value)
        {
            return InternalBuilder.SizeGte(arrayProperty, value);
        }

        public object SizeLt(string arrayProperty, int value)
        {
            return InternalBuilder.SizeLt(arrayProperty, value);
        }

        public object SizeLte(string arrayProperty, int value)
        {
            return InternalBuilder.SizeLte(arrayProperty, value);
        }


        public object Text(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false)
        {
            return InternalBuilder.Text(search, new TextSearchOptions { Language = language, CaseSensitive = caseSensitive, DiacriticSensitive = diacriticSensitive });
        }


        private static FilterDefinition<TDocument> AsMongoFilter(object filter)
        {
            return (FilterDefinition<TDocument>)filter;
        }

        private static IEnumerable<FilterDefinition<TDocument>> AsMongoFilters(IEnumerable<object> filters)
        {
            return filters?.Cast<FilterDefinition<TDocument>>();
        }


        public FilterDefinition<TDocument> CreateMongoFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            return (FilterDefinition<TDocument>)((filter != null) ? filter(this) : Empty());
        }

        public FilterDefinition<TDocument> CreateMongoFilter(Expression<Func<TDocument, bool>> filter)
        {
            return filter ?? EmptyMongoFilter();
        }

        public FilterDefinition<TDocument> EmptyMongoFilter()
        {
            return InternalBuilder.Empty;
        }


        private static Dictionary<DataType, BsonType> BsonTypeMapping =>
            new Dictionary<DataType, BsonType>
            {
                { DataType.Boolean, BsonType.Boolean },
                { DataType.Int32, BsonType.Int32 },
                { DataType.Int64, BsonType.Int64 },
                { DataType.Double, BsonType.Double },
                { DataType.String, BsonType.String },
                { DataType.DateTime, BsonType.DateTime },
                { DataType.Timestamp, BsonType.Timestamp },
                { DataType.Binary, BsonType.Binary },
                { DataType.Object, BsonType.Document },
                { DataType.Array, BsonType.Array }
            };
    }
}
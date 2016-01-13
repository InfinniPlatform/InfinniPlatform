using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Filters.Extensions;
using InfinniPlatform.Sdk.Documents;

using Nest;

using Newtonsoft.Json;

using IFilter = InfinniPlatform.Sdk.Documents.IFilter;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries
{
    /// <summary>
    /// Фабрика для создания объектов, реализующих интерфейс <see cref="Sdk.Documents.IFilter" /> для использования с Nest
    /// </summary>
    public sealed class NestQueryBuilder : INestFilterBuilder
    {
        private const CriteriaType DefaultCriteriaType = CriteriaType.IsEquals;

        private static readonly Dictionary<CriteriaType, Func<string, object, IFilter>> Factories =
            new Dictionary<CriteriaType, Func<string, object, IFilter>>
            {
                { CriteriaType.IsEquals, BuildEqualsQuery },
                { CriteriaType.IsNotEquals, BuildNotEqualsQuery },
                { CriteriaType.IsEmpty, BuildEmptyQuery },
                { CriteriaType.IsNotEmpty, BuildNotEmptyQuery },
                { CriteriaType.IsContains, BuildContainsQuery },
                { CriteriaType.IsNotContains, BuildNotContainsQuery },
                { CriteriaType.IsStartsWith, BuildStartsWithQuery },
                { CriteriaType.IsNotStartsWith, BuildNotStartsWithQuery },
                { CriteriaType.IsEndsWith, BuildEndsWithQuery },
                { CriteriaType.IsNotEndsWith, BuildNotEndsWithQuery },
                { CriteriaType.IsLessThan, BuildLessThanQuery },
                { CriteriaType.IsLessThanOrEquals, BuildLessThanOrEqualsQuery },
                { CriteriaType.IsMoreThan, BuildMoreThanQuery },
                { CriteriaType.IsMoreThanOrEquals, BuildMoreThanOrEqualsQuery },
                { CriteriaType.IsIn, BuildIsInQuery },
                { CriteriaType.FullTextSearch, BuildFullTextSearchQuery },
                { CriteriaType.IsIdIn, BuildIdInListQuery }
            };

        /// <summary>
        /// Создать фильтр для одного поля с указанным значением и методом сравнения
        /// </summary>
        public IFilter Get(string field, object value, CriteriaType compareMethod)
        {
            if (value == null && compareMethod == CriteriaType.IsEquals)
            {
                compareMethod = CriteriaType.IsEmpty;
            }

            if (value == null && compareMethod == CriteriaType.IsNotEquals)
            {
                compareMethod = CriteriaType.IsNotEmpty;
            }

            var elasticField = field.AsElasticField();
            var elasticValue = value.AsElasticValue();

            Func<string, object, IFilter> factory;

            // пробуем найти в словаре фабрику для указанного типа сравнения
            if (Factories.TryGetValue(compareMethod, out factory))
            {
                return factory.Invoke(elasticField, elasticValue);
            }

            // если получить фабрику для данного типа сравнения не удалось и тип сравнения не совпадает с указанным по умолчанию
            // пытаемся получить фабрику для типа сравнения по умолчанию
            if (compareMethod != DefaultCriteriaType && Factories.TryGetValue(DefaultCriteriaType, out factory))
            {
                return factory.Invoke(elasticField, elasticValue);
            }

            return null;
        }

        public IFilter Get(ICalculatedField script, object value)
        {
            throw new NotImplementedException();
        }

        private static IFilter BuildContainsQuery(string field, object value)
        {
            return new NestQuery(Query<IndexObject>.Wildcard(field, string.Format("*{0}*", value)));
        }

        private static IFilter BuildEmptyQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Wildcard(field, "?*"));
        }

        private static IFilter BuildEndsWithQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Wildcard(field, "*" + value.ToString().ToLowerInvariant()));
        }

        private static IFilter BuildEqualsQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Term(field, value));
        }

        private static IFilter BuildFullTextSearchQuery(string field, object value)
        {
            // При индексации токенайзер удаляет дефис и решетку 
            // http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/_finding_exact_values.html#_term_filter_with_numbers
            // Заменяем их пробелы

            var processedValue = "*" + value.ToString().Replace(" ", "?").Replace("-", "?").Replace("#", "?").Trim() + "*";

            //TODO: полнотекстовый поиск будет работать корректно только в случае использования match query:
            //		        return new NestFilter(
            //                    Filter<dynamic>.Query(qs => qs.MultiMatch(s => s.OnFields(new [] {"Values.Street*"}).Query(processedValue).Analyzer("keywordbasedsearch"))));
            //для этого необходимо передавать список полей, по которым выполняется полнотекстовый поиск документов

            if (string.IsNullOrEmpty(field))
            {
                return new NestQuery(
                    Query<dynamic>.QueryString(qs => qs
                                                         .Analyzer("fulltextquery")
                                                         .Query(processedValue)));
            }

            return new NestQuery(
                Query<dynamic>.QueryString(qs => qs
                                                     .Analyzer("fulltextquery")
                                                     .OnFields(field.Split('\n'))
                                                     .Query(processedValue)));
        }

        private static IFilter BuildIdInListQuery(string field, object value)
        {
            var values = JsonConvert.DeserializeObject<IEnumerable<string>>((string)value);

            var nestQuery = new NestQuery(Query<dynamic>.Ids(values));

            return nestQuery;
        }

        private static IFilter BuildIsInQuery(string field, object value)
        {
            var valueSet = value.ToString().Split('\n');

            return new NestQuery(Query<dynamic>.Terms(field, valueSet));
        }

        private static IFilter BuildLessThanQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Range(r => r.OnField(field).Lower(value.ToString())));
        }

        private static IFilter BuildLessThanOrEqualsQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Range(r => r.OnField(field).LowerOrEquals(value.ToString())));
        }

        private static IFilter BuildMoreThanQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Range(r => r.OnField(field).Greater(value.ToString())));
        }

        private static IFilter BuildMoreThanOrEqualsQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Range(r => r.OnField(field).GreaterOrEquals(value.ToString())));
        }

        private static IFilter BuildNotContainsQuery(string field, object value)
        {
            return new NestQuery(!Query<dynamic>.Wildcard(field, $"*{value}*"));
        }

        private static IFilter BuildNotEmptyQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Wildcard(field, "?*"));
        }

        private static IFilter BuildNotEndsWithQuery(string field, object value)
        {
            return new NestQuery(!Query<dynamic>.Wildcard(field, "*" + value.ToString().ToLowerInvariant()));
        }

        private static IFilter BuildNotEqualsQuery(string field, object value)
        {
            return new NestQuery(!Query<dynamic>.Term(field, value.ToString()));
        }

        private static IFilter BuildNotStartsWithQuery(string field, object value)
        {
            return new NestQuery(!Query<dynamic>.Wildcard(field, value + "*"));
        }

        private static IFilter BuildStartsWithQuery(string field, object value)
        {
            return new NestQuery(Query<dynamic>.Wildcard(field, value + "*"));
        }
    }
}
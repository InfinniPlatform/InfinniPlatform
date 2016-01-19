using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Filters.Extensions;
using InfinniPlatform.Sdk.Documents;

using Nest;

using Newtonsoft.Json;

using IFilter = InfinniPlatform.Sdk.Documents.IFilter;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters
{
    /// <summary>
    /// Фабрика для создания объектов, реализующих интерфейс <see cref="Sdk.Documents.IFilter" /> для использования с Nest
    /// </summary>
    public sealed class NestFilterBuilder : INestFilterBuilder
    {
        private const CriteriaType DefaultCriteriaType = CriteriaType.IsEquals;

        private static readonly Dictionary<CriteriaType, Func<string, object, IFilter>> FilterFactories =
            new Dictionary<CriteriaType, Func<string, object, IFilter>>
            {
                { CriteriaType.IsEquals, BuildEqualsFilter },
                { CriteriaType.IsNotEquals, BuildNotEqualsFilter },
                { CriteriaType.IsEmpty, BuildEmptyFilter },
                { CriteriaType.IsNotEmpty, BuildNotEmptyFilter },
                { CriteriaType.IsContains, BuildContainsFilter },
                { CriteriaType.IsNotContains, BuildNotContainsFilter },
                { CriteriaType.IsStartsWith, BuildStartsWithFilter },
                { CriteriaType.IsNotStartsWith, BuildNotStartsWithFilter },
                { CriteriaType.IsEndsWith, BuildEndsWithFilter },
                { CriteriaType.IsNotEndsWith, BuildNotEndsWithFilter },
                { CriteriaType.IsLessThan, BuildLessThanFilter },
                { CriteriaType.IsLessThanOrEquals, BuildLessThanOrEqualsFilter },
                { CriteriaType.IsMoreThan, BuildMoreThanFilter },
                { CriteriaType.IsMoreThanOrEquals, BuildMoreThanOrEqualsFilter },
                { CriteriaType.IsIn, BuildIsInFilter },
                { CriteriaType.FullTextSearch, BuildFullTextSearchFilter },
                { CriteriaType.IsIdIn, BuildIdInListFilter }
            };

        /// <summary>
        /// Создать фильтр для одного поля с указанным значением и методом сравнения
        /// </summary>
        public IFilter Get(string field, object value, CriteriaType compareMethod)
        {
            if (value == null)
            {
                if (compareMethod == CriteriaType.IsEquals)
                {
                    compareMethod = CriteriaType.IsEmpty;
                }

                if (compareMethod == CriteriaType.IsNotEquals)
                {
                    compareMethod = CriteriaType.IsNotEmpty;
                }
            }

            var elasticField = field.AsElasticField();
            var elasticValue = value.AsElasticValue();

            Func<string, object, IFilter> factory;

            // пробуем найти в словаре фабрику для указанного типа сравнения
            if (FilterFactories.TryGetValue(compareMethod, out factory))
            {
                return factory.Invoke(elasticField, elasticValue);
            }

            // если получить фабрику для данного типа сравнения не удалось и тип сравнения не совпадает с указанным по умолчанию
            // пытаемся получить фабрику для типа сравнения по умолчанию
            if (compareMethod != DefaultCriteriaType && FilterFactories.TryGetValue(DefaultCriteriaType, out factory))
            {
                return factory.Invoke(elasticField, elasticValue);
            }

            return null;
        }

        public IFilter Get(ICalculatedField script, object value)
        {
            var rawScript = script.GetRawScript();
            var elasticValue = value.AsElasticValue();

            return BuildScriptFilter(rawScript, elasticValue);
        }

        private static IFilter BuildContainsFilter(string field, object value)
        {
            return new NestFilter(Filter<IndexObject>.Query(q => q.Wildcard(field, $"*{value}*")));
        }

        private static IFilter BuildEmptyFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => !q.Wildcard(field, "?*")));
        }

        private static IFilter BuildEndsWithFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => q.Wildcard(field, $"*{value}".ToLower())));
        }

        private static IFilter BuildEqualsFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => q.Term(field, value)));
        }

        private static IFilter BuildFullTextSearchFilter(string field, object value)
        {
            // При индексации токенайзер удаляет дефис и решетку 
            // http://www.elasticsearch.org/guide/en/elasticsearch/guide/current/_finding_exact_values.html#_term_filter_with_numbers
            // Заменяем их пробелы

            var valueString = value.ToString();

            if (string.IsNullOrEmpty(valueString))
            {
                return null;
            }

            var processedValue = "*" + valueString.Replace(" ", "?").Replace("-", "?").Replace("#", "?").Trim() + "*";

            //TODO: полнотекстовый поиск будет работать корректно только в случае использования match query:
            //		        return new NestFilter(
            //                    Filter<dynamic>.Query(qs => qs.MultiMatch(s => s.OnFields(new [] {"Values.Street*"}).Query(processedValue).Analyzer("keywordbasedsearch"))));
            //для этого необходимо передавать список полей, по которым выполняется полнотекстовый поиск документов

            if (string.IsNullOrEmpty(field))
            {
                return new NestFilter(Filter<dynamic>.Query(q => q.QueryString(qs => qs.Analyzer("fulltextquery")
                                                                                       .Query(processedValue))));
            }

            return new NestFilter(Filter<dynamic>.Query(q => q.QueryString(qs => qs.Analyzer("fulltextquery")
                                                                                   .OnFields(field.Split('\n'))
                                                                                   .Query(processedValue))));
        }

        private static IFilter BuildIdInListFilter(string field, object value)
        {
            var values = JsonConvert.DeserializeObject<IEnumerable<string>>((string)value);

            return new NestFilter(Filter<dynamic>.Ids(values));
        }

        private static IFilter BuildIsInFilter(string field, object value)
        {
            var valueSet = value.ToString().Split('\n');

            return new NestFilter(Filter<dynamic>.Terms(field, valueSet));
        }

        private static IFilter BuildLessThanFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Range(r => r.OnField(field).Lower(value.ToString())));
        }

        private static IFilter BuildLessThanOrEqualsFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Range(r => r.OnField(field).LowerOrEquals(value.ToString())));
        }

        private static IFilter BuildMoreThanFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Range(r => r.OnField(field).Greater(value.ToString())));
        }

        private static IFilter BuildMoreThanOrEqualsFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Range(r => r.OnField(field).GreaterOrEquals(value.ToString())));
        }

        private static IFilter BuildNotContainsFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => !q.Wildcard(field, $"*{value}*")));
        }

        private static IFilter BuildNotEmptyFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => q.Wildcard(field, "?*")));
        }

        private static IFilter BuildNotEndsWithFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => !q.Wildcard(field, "*" + value.ToString().ToLowerInvariant())));
        }

        private static IFilter BuildNotEqualsFilter(string field, object value)
        {
            return new NestFilter(!Filter<dynamic>.Term(field, value.ToString()));
        }

        private static IFilter BuildNotStartsWithFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => !q.Wildcard(field, value + "*")));
        }

        private static IFilter BuildScriptFilter(string script, object value)
        {
            var scriptText = value != null
                                 ? $"{script} == \"{value}\""
                                 : script;
            return new NestFilter(Filter<dynamic>.Script(scriptDescriptor => scriptDescriptor.Script(scriptText)));
        }

        private static IFilter BuildStartsWithFilter(string field, object value)
        {
            return new NestFilter(Filter<dynamic>.Query(q => q.Wildcard(field, value + "*")));
        }
    }
}
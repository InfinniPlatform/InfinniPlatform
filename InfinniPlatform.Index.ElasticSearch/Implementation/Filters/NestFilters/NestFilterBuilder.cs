using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders;
using System.Collections.Generic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters
{
    /// <summary>
    /// Фабрика для создания объектов, реализующих интерфейс <see cref="IFilter"/> для использования с Nest
    /// </summary>
    public sealed class NestFilterBuilder : IFilterBuilder
    {
        private const CriteriaType DefaultCriteriaType = CriteriaType.IsEquals;

        private static readonly Dictionary<CriteriaType, IConcreteFilterBuilder> Factories = new Dictionary
            <CriteriaType, IConcreteFilterBuilder>
        {
            {CriteriaType.IsEquals, new NestFilterEqualsBuilder()},
            {CriteriaType.IsNotEquals, new NestFilterNotEqualsBuilder()},
            {CriteriaType.IsEmpty, new NestFilterEmptyBuilder()},
            {CriteriaType.IsNotEmpty, new NestFilterNotEmptyBuilder()},
            {CriteriaType.IsContains, new NestFilterContainsBuilder()},
            {CriteriaType.IsNotContains, new NestFilterNotContainsBuilder()},
            {CriteriaType.IsStartsWith, new NestFilterStartsWithBuilder()},
            {CriteriaType.IsNotStartsWith, new NestFilterNotStartsWithBuilder()},
            {CriteriaType.IsEndsWith, new NestFilterEndsWithBuilder()},
            {CriteriaType.IsNotEndsWith, new NestFilterNotEndsWithBuilder()},
            {CriteriaType.IsLessThan, new NestFilterLessThanBuilder()},
            {CriteriaType.IsLessThanOrEquals, new NestFilterLessThanOrEqualsBuilder()},
            {CriteriaType.IsMoreThan, new NestFilterMoreThanBuilder()},
            {CriteriaType.IsMoreThanOrEquals, new NestFilterMoreThanOrEqualsBuilder()},
            {CriteriaType.IsIn, new NestFilterIsInBuilder()},
            {CriteriaType.Script, new NestFilterScriptBuilder()},
            {CriteriaType.FullTextSearch, new NestFilterFullTextSearchBuilder()},
			{CriteriaType.IsIdIn, new NestFilterIdInListBuilder()}
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

            IConcreteFilterBuilder factory;

            // пробуем найти в словаре фабрику для указанного типа сравнения
            if (Factories.TryGetValue(compareMethod, out factory))
                return factory.Get(elasticField, elasticValue);

            // если получить фабрику для данного типа сравнения не удалось и тип сравнения не совпадает с указанным по умолчанию
            // пытаемся получить фабрику для типа сравнения по умолчанию
            if (compareMethod != DefaultCriteriaType && Factories.TryGetValue(DefaultCriteriaType, out factory))
                return factory.Get(elasticField, elasticValue);

            return null;
        }

        public IFilter Get(ICalculatedField script, object value)
        {
            var factory = Factories[CriteriaType.Script];
            var elasticValue = value.AsElasticValue();

            return factory.Get(script.GetRawScript(), elasticValue);
        }
    }
}

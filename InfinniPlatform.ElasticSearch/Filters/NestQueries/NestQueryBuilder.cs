using System.Collections.Generic;

using InfinniPlatform.ElasticSearch.Filters.Extensions;
using InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries
{
    /// <summary>
    /// Фабрика для создания объектов, реализующих интерфейс <see cref="IFilter"/> для использования с Nest
    /// </summary>
    public sealed class NestQueryBuilder : IFilterBuilder
    {
        private const CriteriaType DefaultCriteriaType = CriteriaType.IsEquals;

        private static readonly Dictionary<CriteriaType, IConcreteFilterBuilder> Factories = new Dictionary
            <CriteriaType, IConcreteFilterBuilder>
        {
            {CriteriaType.IsEquals, new NestQueryEqualsBuilder()},
            {CriteriaType.IsNotEquals, new NestQueryNotEqualsBuilder()},
            {CriteriaType.IsEmpty, new NestQueryEmptyBuilder()},
            {CriteriaType.IsNotEmpty, new NestQueryNotEmptyBuilder()},
            {CriteriaType.IsContains, new NestQueryContainsBuilder()},
            {CriteriaType.IsNotContains, new NestQueryNotContainsBuilder()},
            {CriteriaType.IsStartsWith, new NestQueryStartsWithBuilder()},
            {CriteriaType.IsNotStartsWith, new NestQueryNotStartsWithBuilder()},
            {CriteriaType.IsEndsWith, new NestQueryEndsWithBuilder()},
            {CriteriaType.IsNotEndsWith, new NestQueryNotEndsWithBuilder()},
            {CriteriaType.IsLessThan, new NestQueryLessThanBuilder()},
            {CriteriaType.IsLessThanOrEquals, new NestQueryLessThanOrEqualsBuilder()},
            {CriteriaType.IsMoreThan, new NestQueryMoreThanBuilder()},
            {CriteriaType.IsMoreThanOrEquals, new NestQueryMoreThanOrEqualsBuilder()},
            {CriteriaType.IsIn, new NestQueryIsInBuilder()},
            //{CriteriaType.Script, new NestQueryScriptBuilder()},
            {CriteriaType.FullTextSearch, new NestQueryFullTextSearchBuilder()},
			{CriteriaType.IsIdIn, new NestQueryIdInListBuilder()}
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

using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Позволяет описать набор критериев
    /// </summary>
    public sealed class FilterBuilder
    {
        public FilterBuilder()
        {
            CriteriaList = new List<FilterCriteria>();
        }

        public IList<FilterCriteria> CriteriaList { get; }

        public FilterBuilder AddCriteria(Action<CriteriaBuilder> criteria)
        {
            var criteriaBuilder = new CriteriaBuilder();

            criteria.Invoke(criteriaBuilder);

            CriteriaList.Add(criteriaBuilder.GetCriteria());

            return this;
        }

        public static IEnumerable<object> SingleCondition(Action<CriteriaBuilder> criteria)
        {
            var builder = new FilterBuilder();

            builder.AddCriteria(criteria);

            return builder.CriteriaList;
        }

        public static IEnumerable<object> DateRangeCondition(string propertyName, DateTime from, DateTime to)
        {
            var builder = new FilterBuilder();

            builder.AddCriteria(c => c.Property(propertyName)
                                      .IsMoreThanOrEquals(from));

            builder.AddCriteria(c => c.Property(propertyName)
                                      .IsLessThanOrEquals(to));

            return builder.CriteriaList;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Core.SearchOptions.Builders
{
    /// <summary>
    /// Позволяет описать набор критериев
    /// </summary>
    public sealed class FilterBuilder
    {
        public FilterBuilder()
        {
            _criteriaList = new List<dynamic>();
        }

        private readonly IList<object> _criteriaList;

        public FilterBuilder AddCriteria(Action<CriteriaBuilder> criteria)
        {
            var criteriaBuilder = new CriteriaBuilder();

            criteria.Invoke(criteriaBuilder);

            _criteriaList.Add(criteriaBuilder.GetCriteria());

            return this;
        }

        public IEnumerable<object> GetFilter()
        {
            return _criteriaList;
        }

        public static IEnumerable<object> SingleCondition(Action<CriteriaBuilder> criteria)
        {
            var builder = new FilterBuilder();

            builder.AddCriteria(criteria);

            return builder.GetFilter();
        }

        public static IEnumerable<object> DateRangeCondition(string propertyName, DateTime from, DateTime to)
        {
            var builder = new FilterBuilder();

            builder.AddCriteria(c => c
                                         .Property(propertyName)
                                         .IsMoreThanOrEquals(from));

            builder.AddCriteria(c => c
                                         .Property(propertyName)
                                         .IsLessThanOrEquals(to));

            return builder.GetFilter();
        }


        public class CriteriaBuilder
        {
            public CriteriaBuilder()
            {
                _criteriaFilter = new CriteriaFilter { CriteriaType = CriteriaType.IsEquals };
            }

            private readonly CriteriaFilter _criteriaFilter;

            public CriteriaBuilder Property(string property)
            {
                _criteriaFilter.Property = property;
                return this;
            }

            public CriteriaBuilder IsEquals(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsEquals;
                return this;
            }

            public CriteriaBuilder IsNotEquals(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsNotEquals;
                return this;
            }

            public CriteriaBuilder IsMoreThan(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsMoreThan;
                return this;
            }

            public CriteriaBuilder IsLessThan(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsLessThan;
                return this;
            }

            public CriteriaBuilder IsMoreThanOrEquals(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsMoreThanOrEquals;
                return this;
            }

            public CriteriaBuilder IsLessThanOrEquals(object value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsLessThanOrEquals;
                return this;
            }

            public CriteriaBuilder IsContains(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsContains;
                return this;
            }

            public CriteriaBuilder IsNotContains(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsNotContains;
                return this;
            }

            public CriteriaBuilder IsEmpty()
            {
                _criteriaFilter.Value = string.Empty;
                _criteriaFilter.CriteriaType = CriteriaType.IsEmpty;
                return this;
            }

            public CriteriaBuilder IsNotEmpty()
            {
                _criteriaFilter.Value = string.Empty;
                _criteriaFilter.CriteriaType = CriteriaType.IsNotEmpty;
                return this;
            }

            public CriteriaBuilder IsStartsWith(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsStartsWith;
                return this;
            }

            public CriteriaBuilder IsNotStartsWith(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsNotStartsWith;
                return this;
            }

            public CriteriaBuilder IsEndsWith(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsEndsWith;
                return this;
            }

            public CriteriaBuilder IsNotEndsWith(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.IsNotEndsWith;
                return this;
            }

            public CriteriaBuilder Script(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.Script;
                return this;
            }

            public CriteriaBuilder FullTextSearch(string value)
            {
                _criteriaFilter.Value = value;
                _criteriaFilter.CriteriaType = CriteriaType.FullTextSearch;
                return this;
            }

            public CriteriaBuilder IsIn(params object[] values)
            {
                _criteriaFilter.Value = string.Join("\n", values.Select(v => v.ToString()));
                _criteriaFilter.CriteriaType = CriteriaType.IsIn;
                return this;
            }

            public CriteriaBuilder IsIdIn(List<string> idList)
            {
                _criteriaFilter.Value = idList;
                _criteriaFilter.CriteriaType = CriteriaType.IsIdIn;
                return this;
            }

            internal object GetCriteria()
            {
                return _criteriaFilter;
            }
        }


        public sealed class CriteriaFilter
        {
            public string Property { get; set; }

            public object Value { get; set; }

            public CriteriaType CriteriaType { get; set; }
        }
    }
}
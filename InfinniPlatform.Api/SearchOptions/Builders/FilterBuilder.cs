using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.SearchOptions.Builders
{
    /// <summary>
    /// Позволяет описать набор критериев
    /// </summary>
    public sealed class FilterBuilder
    {
        private readonly IList<object> _criteriaList;

	    public FilterBuilder()
        {
            _criteriaList = new List<dynamic>();
        }

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

        /// <summary>
        /// Позволяет задать одно условие
        /// </summary>
        public class CriteriaBuilder
        {
            private string _property;

            private object _value;

            private CriteriaType _criteriaType = CriteriaType.IsEquals;

            public CriteriaBuilder Property(string property)
            {
                _property = property;
                return this;
            }

            public CriteriaBuilder IsEquals(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsEquals;
                return this;
            }

            public CriteriaBuilder IsNotEquals(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsNotEquals;
                return this;
            }

            public CriteriaBuilder IsMoreThan(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsMoreThan;
                return this;
            }

            public CriteriaBuilder IsLessThan(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsLessThan;
                return this;
            }

            public CriteriaBuilder IsMoreThanOrEquals(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsMoreThanOrEquals;
                return this;
            }

            public CriteriaBuilder IsLessThanOrEquals(object value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsLessThanOrEquals;
                return this;
            }

            public CriteriaBuilder IsContains(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsContains;
                return this;
            }

            public CriteriaBuilder IsNotContains(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsNotContains;
                return this;
            }

            public CriteriaBuilder IsEmpty()
            {
                _value = string.Empty;
                _criteriaType = CriteriaType.IsEmpty;
                return this;
            }

            public CriteriaBuilder IsNotEmpty()
            {
                _value = string.Empty;
                _criteriaType = CriteriaType.IsNotEmpty;
                return this;
            }

            public CriteriaBuilder IsStartsWith(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsStartsWith;
                return this;
            }

            public CriteriaBuilder IsNotStartsWith(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsNotStartsWith;
                return this;
            }

            public CriteriaBuilder IsEndsWith(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsEndsWith;
                return this;
            }

            public CriteriaBuilder IsNotEndsWith(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.IsNotEndsWith;
                return this;
            }

            public CriteriaBuilder Script(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.Script;
                return this;
            }

            public CriteriaBuilder FullTextSearch(string value)
            {
                _value = value;
                _criteriaType = CriteriaType.FullTextSearch;
                return this;
            }

            public CriteriaBuilder IsIn(params object[] values)
            {
                _value = string.Join("\n", values.Select(v => v.ToString()));
                _criteriaType = CriteriaType.IsIn;
                return this;
            }

            internal object GetCriteria()
            {
                return new
                {
                    Property = _property,
                    Value = _value,
                    CriteriaType = _criteriaType
                };
            }

	        public CriteriaBuilder IsIdIn(List<string> idList)
	        {
				_value = idList;
				_criteriaType = CriteriaType.IsIdIn;
				return this;
	        }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    /// Позволяет описать набор критериев
    /// </summary>
    public sealed class FilterBuilder
    {
        private readonly IList<string> _criteriaList;

        public FilterBuilder()
        {
            _criteriaList = new List<string>();
        }

        public FilterBuilder AddCriteria(Action<CriteriaBuilder> criteria)
        {
            var criteriaBuilder = new CriteriaBuilder();

            criteria.Invoke(criteriaBuilder);

            _criteriaList.Add(criteriaBuilder.GetCriteria());

            return this;
        }

        public IEnumerable<string> GetFilter()
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

            private string _criteriaType = "IsEquals";

            public CriteriaBuilder Property(string property)
            {
                _property = property;
                return this;
            }

            public CriteriaBuilder IsEquals(object value)
            {
                _value = value;
                _criteriaType = "IsEquals";
                return this;
            }

            public CriteriaBuilder IsNotEquals(object value)
            {
                _value = value;
                _criteriaType = "IsNotEquals";
                return this;
            }

            public CriteriaBuilder IsMoreThan(object value)
            {
                _value = value;
                _criteriaType = "IsMoreThan";
                return this;
            }

            public CriteriaBuilder IsLessThan(object value)
            {
                _value = value;
                _criteriaType = "IsLessThan";
                return this;
            }

            public CriteriaBuilder IsMoreThanOrEquals(object value)
            {
                _value = value;
                _criteriaType = "IsMoreThanOrEquals";
                return this;
            }

            public CriteriaBuilder IsLessThanOrEquals(object value)
            {
                _value = value;
                _criteriaType = "IsLessThanOrEquals";
                return this;
            }

            public CriteriaBuilder IsContains(string value)
            {
                _value = value;
                _criteriaType = "IsContains";
                return this;
            }

            public CriteriaBuilder IsNotContains(string value)
            {
                _value = value;
                _criteriaType = "IsNotContains";
                return this;
            }

            public CriteriaBuilder IsEmpty()
            {
                _value = string.Empty;
                _criteriaType = "IsEmpty";
                return this;
            }

            public CriteriaBuilder IsNotEmpty()
            {
                _value = string.Empty;
                _criteriaType = "IsNotEmpty";
                return this;
            }

            public CriteriaBuilder IsStartsWith(string value)
            {
                _value = value;
                _criteriaType = "IsStartsWith";
                return this;
            }

            public CriteriaBuilder IsNotStartsWith(string value)
            {
                _value = value;
                _criteriaType = "IsNotStartsWith";
                return this;
            }

            public CriteriaBuilder IsEndsWith(string value)
            {
                _value = value;
                _criteriaType = "IsEndsWith";
                return this;
            }

            public CriteriaBuilder IsNotEndsWith(string value)
            {
                _value = value;
                _criteriaType = "IsNotEndsWith";
                return this;
            }

            public CriteriaBuilder Script(string value)
            {
                _value = value;
                _criteriaType = "Script";
                return this;
            }

            public CriteriaBuilder FullTextSearch(string value)
            {
                _value = value;
                _criteriaType = "FullTextSearch";
                return this;
            }

            public CriteriaBuilder IsIn(params object[] values)
            {
                _value = string.Join("\n", values.Select(v => v.ToString()));
                _criteriaType = "IsIn";
                return this;
            }

            public CriteriaBuilder IsIdIn(List<string> idList)
            {
                _value = idList;
                _criteriaType = "IsIdIn";
                return this;
            }

            internal string GetCriteria()
            {
                return string.Format("{0} {1} {2}", _property, _criteriaType, _value ?? "null");
            }


        }
    }
}

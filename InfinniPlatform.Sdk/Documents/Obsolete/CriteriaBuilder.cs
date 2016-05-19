using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents.Obsolete
{
    /// <summary>
    /// Позволяет задать одно условие
    /// </summary>
    public class CriteriaBuilder
    {
        public FilterCriteria Criteria { get; } = new FilterCriteria(null, null, CriteriaType.IsEquals);

        public CriteriaBuilder Property(string property)
        {
            Criteria.Property = property;

            return this;
        }

        public CriteriaBuilder IsEquals(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsEquals;

            return this;
        }

        public CriteriaBuilder IsNotEquals(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsNotEquals;

            return this;
        }

        public CriteriaBuilder IsMoreThan(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsMoreThan;

            return this;
        }

        public CriteriaBuilder IsLessThan(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsLessThan;

            return this;
        }

        public CriteriaBuilder IsMoreThanOrEquals(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsMoreThanOrEquals;

            return this;
        }

        public CriteriaBuilder IsLessThanOrEquals(object value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsLessThanOrEquals;

            return this;
        }

        public CriteriaBuilder IsContains(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsContains;

            return this;
        }

        public CriteriaBuilder IsNotContains(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsNotContains;

            return this;
        }

        public CriteriaBuilder IsEmpty()
        {
            Criteria.Value = string.Empty;
            Criteria.CriteriaType = CriteriaType.IsEmpty;

            return this;
        }

        public CriteriaBuilder IsNotEmpty()
        {
            Criteria.Value = string.Empty;
            Criteria.CriteriaType = CriteriaType.IsNotEmpty;

            return this;
        }

        public CriteriaBuilder IsStartsWith(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsStartsWith;

            return this;
        }

        public CriteriaBuilder IsNotStartsWith(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsNotStartsWith;

            return this;
        }

        public CriteriaBuilder IsEndsWith(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsEndsWith;

            return this;
        }

        public CriteriaBuilder IsNotEndsWith(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.IsNotEndsWith;

            return this;
        }

        public CriteriaBuilder Script(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.Script;

            return this;
        }

        public CriteriaBuilder FullTextSearch(string value)
        {
            Criteria.Value = value;
            Criteria.CriteriaType = CriteriaType.FullTextSearch;

            return this;
        }

        public CriteriaBuilder IsIn(IEnumerable<string> values)
        {
            Criteria.Value = values;
            Criteria.CriteriaType = CriteriaType.IsIn;

            return this;
        }

        public CriteriaBuilder IsIdIn(IEnumerable<string> idList)
        {
            Criteria.Value = idList;
            Criteria.CriteriaType = CriteriaType.IsIdIn;

            return this;
        }
    }
}
using System;
using System.Diagnostics;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Documents
{
    [Serializable]
    [DebuggerDisplay("{Property}, {CriteriaType}, {Value}")]
    public sealed class FilterCriteria
    {
        public FilterCriteria(string property, object value, CriteriaType criteriaType)
        {
            Property = property;
            Value = value;
            CriteriaType = criteriaType;
        }

        public string Property { get; set; }

        public object Value { get; set; }

        public CriteriaType CriteriaType { get; set; }
    }
}
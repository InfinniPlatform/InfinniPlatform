using System;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Documents
{
    [Serializable]
    public sealed class CriteriaFilter
    {
        public CriteriaFilter(string property, object value, CriteriaType criteriaType)
        {
            Property = property;
            Value = value;
            CriteriaType = criteriaType;
        }

        public string Property { get; set; }

        public object Value { get; set; }

        public CriteriaType CriteriaType { get; set; }

        public string ToJsonString()
        {
            var criteriaFilter = new CriteriaFilter(Property, Value, CriteriaType);
            var serializeObject = JsonConvert.SerializeObject(criteriaFilter);

            return serializeObject;
        }
    }
}
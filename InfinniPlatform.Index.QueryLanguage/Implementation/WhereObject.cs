using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public sealed class WhereObject
    {
        public string Property { get; set; }
        public string RawProperty { get; set; }
        public object Value { get; set; }
        public CriteriaType CriteriaType { get; set; }

        public Criteria ToCriteria()
        {
            return new Criteria
            {
                CriteriaType = CriteriaType,
                Value = Value,
                Property = Property
            };
        }
    }
}
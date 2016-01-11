using InfinniPlatform.Core.Index.SearchOptions;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.QueryLanguage
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
using System.Collections.Generic;
using InfinniPlatform.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
    public interface IFilterBuilderInfoConstructor
    {
        IEnumerable<FilterBuilderInfo> CreateFilterBuilderInfo(IEnumerable<Criteria> criteria);
    }
}
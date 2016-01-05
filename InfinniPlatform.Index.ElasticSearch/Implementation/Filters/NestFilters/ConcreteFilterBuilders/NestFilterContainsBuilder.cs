using InfinniPlatform.Core.Index;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterContainsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<IndexObject>.Query(q => q.Wildcard(field, string.Format("*{0}*", value))));
        }
    }
}

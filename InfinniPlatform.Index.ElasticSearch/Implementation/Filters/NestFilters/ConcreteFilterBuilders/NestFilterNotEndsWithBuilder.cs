using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterNotEndsWithBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<dynamic>.Query(q => !q.Wildcard(field, "*" + value.ToString().ToLowerInvariant())));
        }
    }
}

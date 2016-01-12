using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterEqualsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<dynamic>.Query(q => q.Term(field, value)));
        }
    }
}

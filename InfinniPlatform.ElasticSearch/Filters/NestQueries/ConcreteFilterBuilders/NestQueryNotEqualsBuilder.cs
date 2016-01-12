using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryNotEqualsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(!Nest.Query<dynamic>.Term(field, value.ToString()));
        }
    }
}

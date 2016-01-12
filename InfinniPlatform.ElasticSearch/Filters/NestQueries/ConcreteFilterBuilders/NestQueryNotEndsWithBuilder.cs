using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryNotEndsWithBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(!Nest.Query<dynamic>.Wildcard(field, "*" + value.ToString().ToLowerInvariant()));
        }
    }
}

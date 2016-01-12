using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryNotContainsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(!Nest.Query<dynamic>.Wildcard(field, string.Format("*{0}*", value)));
        }
    }
}

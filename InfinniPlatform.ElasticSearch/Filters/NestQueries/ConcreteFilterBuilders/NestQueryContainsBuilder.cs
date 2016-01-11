using InfinniPlatform.Core.Index;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryContainsBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(Nest.Query<IndexObject>.Wildcard(field, string.Format("*{0}*", value)));
        }
    }
}

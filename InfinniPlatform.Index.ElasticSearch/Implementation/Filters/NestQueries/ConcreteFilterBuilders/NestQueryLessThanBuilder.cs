using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestQueries.ConcreteFilterBuilders
{
    public sealed class NestQueryLessThanBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(Nest.Query<dynamic>.Range(r => r.OnField(field).Lower(value.ToString())));
        }
    }
}

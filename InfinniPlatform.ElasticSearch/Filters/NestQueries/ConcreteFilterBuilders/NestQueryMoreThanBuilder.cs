using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryMoreThanBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestQuery(Nest.Query<dynamic>.Range(r => r.OnField(field).Greater(value.ToString())));
        }
    }
}

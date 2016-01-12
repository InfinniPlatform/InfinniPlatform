using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    public sealed class NestFilterLessThanBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            return new NestFilter(Nest.Filter<dynamic>.Range(r => r.OnField(field).Lower(value.ToString())));
        }
    }
}

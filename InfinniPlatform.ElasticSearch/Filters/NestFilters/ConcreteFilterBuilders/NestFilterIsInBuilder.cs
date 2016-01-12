using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    internal sealed class NestFilterIsInBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            var valueSet = value.ToString().Split('\n');

            return new NestFilter(Nest.Filter<dynamic>.Terms(field, valueSet));
        }
    }
}

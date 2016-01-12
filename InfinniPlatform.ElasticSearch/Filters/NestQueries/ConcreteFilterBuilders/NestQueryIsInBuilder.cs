using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.Filters.NestQueries.ConcreteFilterBuilders
{
    internal sealed class NestQueryIsInBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            var valueSet = value.ToString().Split('\n');

            return new NestQuery(Nest.Query<dynamic>.Terms(field, valueSet));
        }
    }
}

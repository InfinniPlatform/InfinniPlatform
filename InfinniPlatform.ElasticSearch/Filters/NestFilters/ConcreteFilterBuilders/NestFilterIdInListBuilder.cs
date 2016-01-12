using System.Collections.Generic;

using Nest;

using Newtonsoft.Json;

using IFilter = InfinniPlatform.Sdk.Environment.Index.IFilter;

namespace InfinniPlatform.ElasticSearch.Filters.NestFilters.ConcreteFilterBuilders
{
    public sealed class NestFilterIdInListBuilder : IConcreteFilterBuilder
    {
        public IFilter Get(string field, object value)
        {
            var values = JsonConvert.DeserializeObject<IEnumerable<string>>((string)value);

            var nestFilter = new NestFilter(Filter<dynamic>.Ids(values));

            return nestFilter;
        }
    }
}
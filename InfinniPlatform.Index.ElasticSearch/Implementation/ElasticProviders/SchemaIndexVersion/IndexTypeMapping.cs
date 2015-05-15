using System.Collections.Generic;
using InfinniPlatform.Api.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    public sealed class IndexTypeMapping : IIndexTypeMapping
    {
        public IndexTypeMapping(
            IList<PropertyMapping> properties)
        {
            Properties = properties ?? new List<PropertyMapping>();
        }

        /// <summary>
        /// Список полей типа
        /// </summary>
        public IList<PropertyMapping> Properties { get; private set; }
    }
}
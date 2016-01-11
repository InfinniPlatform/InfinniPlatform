using System.Collections.Generic;

using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion
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
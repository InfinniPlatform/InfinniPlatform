using System.Collections.Generic;

using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    public class IndexToTypeAccordanceProvider
    {
        public IndexToTypeAccordanceProvider(ElasticTypeManager elasticTypeManager)
        {
            _elasticTypeManager = elasticTypeManager;
        }

        private readonly ElasticTypeManager _elasticTypeManager;

        public IndexToTypeAccordanceSettings GetIndexTypeAccordances(string indexName, string typeName)
        {
            var indexTypesNest = _elasticTypeManager.GetTypeMappings(indexName, typeName);

            var indexEmpty = string.IsNullOrEmpty(indexName);

            return new IndexToTypeAccordanceSettings(new Dictionary<string, IEnumerable<TypeMapping>> { { indexName, indexTypesNest } }, indexEmpty);
        }
    }
}
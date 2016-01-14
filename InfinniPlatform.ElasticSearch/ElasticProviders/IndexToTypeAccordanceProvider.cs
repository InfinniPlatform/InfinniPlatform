using System.Collections.Generic;

using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    public class IndexToTypeAccordanceProvider
    {
        public IndexToTypeAccordanceProvider(ElasticTypeManager elasticTypeManager)
        {
            _elasticTypeManager = elasticTypeManager;
        }

        private readonly ElasticTypeManager _elasticTypeManager;

        public Dictionary<string, IEnumerable<TypeMapping>> GetIndexTypeAccordances(string indexName, string typeName)
        {
            var indexTypesNest = _elasticTypeManager.GetTypeMappings(indexName, typeName);

            return new Dictionary<string, IEnumerable<TypeMapping>> { { indexName, indexTypesNest } };
        }
    }
}
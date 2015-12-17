using System.Collections.Generic;

using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    public class IndexToTypeAccordanceProvider
    {
        public IndexToTypeAccordanceProvider()
        {
            _elasticConnection = new ElasticConnection();
        }

        private readonly ElasticConnection _elasticConnection;

        public IndexToTypeAccordanceSettings GetIndexTypeAccordances(string indexName, IEnumerable<string> typeNames)
        {
            var indexTypesNest = _elasticConnection.GetTypeMappings(indexName, typeNames);

            var indexEmpty = string.IsNullOrEmpty(indexName);

            return new IndexToTypeAccordanceSettings(new Dictionary<string, IEnumerable<TypeMapping>> { { indexName, indexTypesNest } }, indexEmpty);
        }
    }
}
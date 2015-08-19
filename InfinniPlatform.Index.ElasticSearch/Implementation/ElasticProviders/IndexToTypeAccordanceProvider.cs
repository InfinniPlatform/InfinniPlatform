using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    public class IndexToTypeAccordanceProvider
    {
        private readonly ElasticConnection _elasticConnection;
        
        public IndexToTypeAccordanceProvider()
        {
            _elasticConnection = new ElasticConnection();
        }

        public IndexToTypeAccordanceSettings GetIndexTypeAccordances(IEnumerable<string> indeces,
            IEnumerable<string> types)
        {            
            return new IndexToTypeAccordanceSettings(_elasticConnection.GetAllTypes(indeces, types), indeces != null && indeces.Any());
        } 
    }
}

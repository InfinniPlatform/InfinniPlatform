using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    public class IndexToTypeAccordanceSettings
    {
        private readonly IEnumerable<IndexToTypeAccordance> _accordances;

        public IndexToTypeAccordanceSettings(IEnumerable<IndexToTypeAccordance> accordances)
        {
            _accordances = accordances;
        }

        public bool SearchInAllTypes
        {
            get
            {
                return !_accordances.Any();
            }
        }

        public bool SearchInAllIndeces
        {
            get
            {
                return !_accordances.Any();
            }
        }

        public IEnumerable<IndexToTypeAccordance> Accordances
        {
            get { return _accordances; }
        } 
    }
}

using System.Collections.Generic;
using System.Linq;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    public class IndexToTypeAccordanceSettings
    {
        public IndexToTypeAccordanceSettings(Dictionary<string, IEnumerable<TypeMapping>> accordances, bool indexEmpty)
        {
            Accordances = accordances;
            _indexEmpty = indexEmpty;
        }

        private readonly bool _indexEmpty;

        public bool SearchInAllTypes => !_indexEmpty && Accordances.Values.Any();

        public bool SearchInAllIndeces => _indexEmpty && !Accordances.Values.Any();

        public Dictionary<string, IEnumerable<TypeMapping>> Accordances { get; }
    }
}
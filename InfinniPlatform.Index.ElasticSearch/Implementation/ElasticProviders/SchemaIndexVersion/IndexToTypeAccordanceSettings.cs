using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
	public class IndexToTypeAccordanceSettings
	{
		private readonly IEnumerable<IndexToTypeAccordance> _accordances;
		private readonly bool _indexEmpty;

		public IndexToTypeAccordanceSettings(IEnumerable<IndexToTypeAccordance> accordances, bool indexEmpty)
		{
			_accordances = accordances;
			_indexEmpty = indexEmpty;
		}

		public bool SearchInAllTypes
		{
			get
			{
				return !_indexEmpty && Accordances.Select(s => s.TypeNames).Any();
			}
		}

		public bool SearchInAllIndeces
		{
			get
			{
				return _indexEmpty && !Accordances.Select(s => s.TypeNames).Any();
			}
		}

		public IEnumerable<IndexToTypeAccordance> Accordances
		{
			get { return _accordances; }
		}
	}
}
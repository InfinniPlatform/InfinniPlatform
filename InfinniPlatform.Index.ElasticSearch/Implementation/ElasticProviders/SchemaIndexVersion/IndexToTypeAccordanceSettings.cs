using System.Collections.Generic;
using System.Linq;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
	public class IndexToTypeAccordanceSettings
	{
	    private readonly bool _indexEmpty;

		public IndexToTypeAccordanceSettings(Dictionary<string, IList<TypeMapping>> accordances, bool indexEmpty)
		{
			Accordances = accordances;
			_indexEmpty = indexEmpty;
		}

		public bool SearchInAllTypes
		{
			get
			{
			    return true;
			    //				return !_indexEmpty && Accordances.Select(s => s.TypeNames).Any();
			}
		}

		public bool SearchInAllIndeces
		{
			get
			{
                return true;
                //				return _indexEmpty && !Accordances.Select(s => s.TypeNames).Any();
            }
		}

		public Dictionary<string, IList<TypeMapping>> Accordances { get; }
	}
}
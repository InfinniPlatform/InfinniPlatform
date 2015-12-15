using System.Collections.Generic;
using System.Linq;

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

		public IndexToTypeAccordanceSettings GetIndexTypeAccordances(string index, IEnumerable<string> types)
		{
//			var indexTypes = _elasticConnection.GetAllTypes(indeces, types);
            var indexTypesNest = _elasticConnection.GetAllTypesNest(index, types);
            var indexEmpty = string.IsNullOrEmpty(index);

			return new IndexToTypeAccordanceSettings(indexTypesNest, indexEmpty);
		}
	}
}
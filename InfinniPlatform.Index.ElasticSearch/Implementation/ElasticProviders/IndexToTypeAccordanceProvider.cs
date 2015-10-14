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

		public IndexToTypeAccordanceSettings GetIndexTypeAccordances(IEnumerable<string> indeces, IEnumerable<string> types)
		{
			var indexTypes = _elasticConnection.GetAllTypes(indeces, types);
			var indexEmpty = indeces != null && indeces.Any();

			return new IndexToTypeAccordanceSettings(indexTypes, indexEmpty);
		}
	}
}
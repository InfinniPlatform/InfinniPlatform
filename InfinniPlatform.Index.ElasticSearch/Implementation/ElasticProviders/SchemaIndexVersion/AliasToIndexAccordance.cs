using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
	/// <summary>
	///   Соответствие алиаса индекса списку индексов
	///   Например: алиасу product соответствует следующая схема маппинга
	///  			  product_indexschema_0": {  -- наименование индекса
	//					"product_typeschema_0": {...}, -- наименование типа
	//					"product_typeschema_1": {...} -- наименование типа
	//					}
	//				 },
	/// </summary>
	public sealed class AliasToIndexAccordance
	{
		private readonly List<IndexToTypeAccordance> _indexToTypeAccordances = new List<IndexToTypeAccordance>();

		public string AliasName { get; set; }

		public IEnumerable<IndexToTypeAccordance> IndexToTypeAccordances
		{
			get { return _indexToTypeAccordances; }
		}

		public void RegisterIndexTypeAccordances(IEnumerable<IndexToTypeAccordance> indexToTypeAccordances)
		{
			_indexToTypeAccordances.AddRange(indexToTypeAccordances);
		}
	}
}

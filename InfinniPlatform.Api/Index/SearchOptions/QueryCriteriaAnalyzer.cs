using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.Api.Index.SearchOptions
{
	/// <summary>
	///   Анализатор критериев запроса данных
	/// </summary>
	public sealed class QueryCriteriaAnalyzer
	{
		private readonly IMetadataComponent _metadataComponent;
		private readonly IEnumerable<dynamic> _filter;
		private readonly dynamic _schema;
		private SchemaIterator _metadataIterator;
		private List<SchemaObject> resolveProperties = new List<SchemaObject>(); 


		/// <summary>
		///   Конструктор
		/// </summary>
		/// <param name="metadataComponent">Провайдер метаданных сервисной части</param>
		/// <param name="schema">Схема данных документа, к которому выполняется запрос</param>
		public QueryCriteriaAnalyzer(IMetadataComponent metadataComponent, dynamic schema)
		{
			_metadataComponent = metadataComponent;
			_schema = schema;
			_metadataIterator = new SchemaIterator(new SchemaProvider(metadataComponent));
			_metadataIterator.OnObjectProperty = schemaObject =>
				                                     {
														 if (schemaObject.IsResolve)
														 {
															 resolveProperties.Add(schemaObject);
														 }
				                                     };
			_metadataIterator.OnArrayProperty = schemaObject =>
				                                    {
					                                    if (schemaObject.IsDocumentArray)
					                                    {
						                                    resolveProperties.Add(schemaObject);
					                                    }
				                                    };
			_metadataIterator.ProcessSchema(_schema);
		}

		/// <summary>
		///   Получить список критериев документа, применяемых до выполнения операции Resolve
		/// </summary>
		/// <param name="filter">Список критериев запроса</param>
		/// <returns>Список критериев</returns>
		public IEnumerable<dynamic> GetBeforeResolveCriteriaList(IEnumerable<dynamic> filter)
		{
			var result = new List<dynamic>();
			foreach (dynamic criteria in filter)
			{
				if (resolveProperties.Any(r => ((string) criteria.Property).StartsWith(r.GetFullPath())))
				{
					continue;					
				}
				result.Add(criteria);
			}
			return result;
		}


		/// <summary>
		///   Получить список критериев запроса, выполняемых после операции Resolve 
		/// </summary>
		/// <param name="filter">Список критериев запроса</param>
		/// <returns>Список критериев</returns>
		public IEnumerable<dynamic> GetAfterResolveCriteriaList(IEnumerable<dynamic> filter)
		{
			return filter.Except(GetBeforeResolveCriteriaList(filter));
		}
	}
}

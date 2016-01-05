using System.Linq;
using System.Text.RegularExpressions;
using InfinniPlatform.Core.Index.SearchOptions;
using InfinniPlatform.Sdk.Environment.Index;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
	public class JsonQueryExecutor
	{
		private readonly IIndexFactory _indexFactory;
		private readonly ReferenceBuilder _referenceBuilder;
		private readonly ProjectionBuilder _projectionBuilder;
        private readonly IFilterBuilder _filterFactory;
		
		private readonly Regex _elasticFilterRegex = new Regex(@"^[a-zA-Z\.]+$");

        public JsonQueryExecutor(IIndexFactory indexFactory, IFilterBuilder filterFactory)
		{
			_indexFactory = indexFactory;
		    _filterFactory = filterFactory;
	        _referenceBuilder = new ReferenceBuilder(_indexFactory);
			_projectionBuilder = new ProjectionBuilder();			
		}

		public JArray ExecuteQuery(JObject query)
		{
			

			var queryTree = new QuerySyntaxTree(query);

			//Получаем список условий фильтрации
			var where = queryTree.GetConditionCriteria().ToArray();

			//Получаем индекс для поиска основного объекта
			var from = queryTree.GetFrom();

			var queryExecutor = _indexFactory.BuildIndexQueryExecutor(@from.Index.ToLowerInvariant(),@from.Type.ToLowerInvariant());

			//модель поиска для выборки из индекса основного объекта
			var searchBuilder = new SearchModelBuilder(_filterFactory);

		    var limits = queryTree.GetLimits();
		    if (limits != null)
		    {
		        searchBuilder.FromPage(limits.StartPage);
		        searchBuilder.PageSize(limits.PageSize);
		        searchBuilder.Skip(limits.Skip);
		    }

			var elasticFields = where.Where(x => _elasticFilterRegex.IsMatch(x.Property));
			var notAliasedElasticFields = QuerySyntaxTree.GetNotAliasedFields(queryTree.GetReferenceObjects().Select(r => r.Alias),
			                                                            elasticFields.Select(e => e.Property));
			var fieldsToFilter = where.Where(wh => notAliasedElasticFields.Contains(wh.Property)) ;



            // в фильтры запроса к эластику включаются только те ограничения, в которых имя поля может быть интерпретировано как поле документа elastcsearch
			foreach (var filter in fieldsToFilter)
		    {
		        searchBuilder.Filter(filter.Property, filter.Value, filter.CriteriaType);
		    }

			//выполняем поиск по индексу
		     var foundObjects = queryExecutor
		        .QueryAsJObject(searchBuilder.BuildQuery())
		        .Items
		        .ToArray();

			//для каждого из объектов выполняем разрешение ссылок
			foreach (var foundObject in foundObjects)
			{
				_referenceBuilder.FillReference(foundObject, queryTree.GetReferenceObjects().ToArray(), queryTree.GetWhereObjects(), _filterFactory);	
			}


			var resultObject = new JArray();

			//для каждого из найденных объектов получаем проекции результата исходя из условий выборки
			foreach (var foundObject in foundObjects)
			{
			    var projection = _projectionBuilder.GetProjection(foundObject, queryTree.GetProjectionObjects(), queryTree.GetWhereObjects(), queryTree.GetReferenceObjects());
                resultObject.Add(projection);
			}

			return resultObject;
		}

	}
}
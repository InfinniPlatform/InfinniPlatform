using System.Linq;
using InfinniPlatform.Factories;
using InfinniPlatform.Index;
using InfinniPlatform.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage
{
	public class JsonQueryExecutor
	{
		private readonly IIndexFactory _indexFactory;
		private readonly ReferenceBuilder _referenceBuilder;
		private readonly ProjectionBuilder _projectionBuilder;

		public JsonQueryExecutor(IIndexFactory indexFactory)
		{
			_indexFactory = indexFactory;
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

			var queryExecutor = _indexFactory.BuildIndexQueryExecutor(from.Index.ToLowerInvariant());

			//модель поиска для выборки из индекса основного объекта
			var searchBuilder = new SearchModelBuilder();

			searchBuilder.AddCriteriaRange(where.GetElasticCriteria());

			//выполняем поиск по индексу
            var foundObjects = queryExecutor.QueryAsJObject(searchBuilder.BuildQuery()).Items.ToArray();

			//для каждого из объектов выполняем разрешение ссылок
			foreach (var foundObject in foundObjects)
			{
				_referenceBuilder.FillReference(foundObject, queryTree.GetReferenceObjects().ToArray());	
			}


			var resultObject = new JArray();

			//для каждого из найденных объектов получаем проекции результата исходя из условий выборки
			foreach (var foundObject in foundObjects)
			{
				resultObject.Add(_projectionBuilder.GetProjection(foundObject, queryTree.GetProjectionObjects(), where));
			}

			return resultObject;
		}

	}
}
using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitGetByQuery
	{
	    private static JsonQueryExecutor _jsonQueryExecutor;

	    public void Action(IApplyResultContext target)
	    {
			IFilterBuilder filterFactory = FilterBuilderFactory.GetInstance();


			_jsonQueryExecutor = new JsonQueryExecutor(target.Context.GetComponent<IIndexComponent>().IndexFactory, filterFactory, 
				target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);

            dynamic query = DynamicWrapperExtensions.ToDynamic((string)target.Item.QueryText);

	        var denormalizeFlag = target.Item.DenormalizeResult;

	        bool denormalize = denormalizeFlag != null && denormalizeFlag == true;

	        if (query.Select == null)
	        {
	            query.Select = new List<dynamic>();
	        }

	        if (query.Where == null)
	        {
	            query.Where = new List<dynamic>();
	        }

	        JArray queryResult = _jsonQueryExecutor.ExecuteQuery(JObject.FromObject(query));

	        target.Result = denormalize
	            ? new JsonDenormalizer().ProcessIqlResult(queryResult).ToDynamic()
	            : queryResult.ToDynamic();
	    }
	}
}

using System.Collections.Generic;

using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.ActionUnits
{
    public sealed class ActionUnitGetByQuery
    {
        private readonly IIndexComponent _indexComponent;

        public ActionUnitGetByQuery(IIndexComponent indexComponent)
        {
            _indexComponent = indexComponent;
        }

        public void Action(IApplyResultContext target)
        {
            var filterFactory = FilterBuilderFactory.GetInstance();

            dynamic query = ((string) target.Item.QueryText).ToDynamic();

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

            JArray queryResult = new JsonQueryExecutor(_indexComponent.IndexFactory, filterFactory).ExecuteQuery(JObject.FromObject(query));

            target.Result = denormalize
                                ? new JsonDenormalizer().ProcessIqlResult(queryResult).ToDynamic()
                                : queryResult.ToDynamic();
        }
    }
}
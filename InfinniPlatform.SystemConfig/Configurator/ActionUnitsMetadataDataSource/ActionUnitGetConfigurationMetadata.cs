using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetConfigurationMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery = new DynamicWrapper();
            bodyQuery.ConfigId = target.Item.ConfigId;

            dynamic response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            IEnumerable<dynamic> result = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
            target.Result = result.Select(r => r.Result).FirstOrDefault();
        }
    }
}
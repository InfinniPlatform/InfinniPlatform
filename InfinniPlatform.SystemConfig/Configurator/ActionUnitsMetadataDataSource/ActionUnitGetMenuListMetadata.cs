using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetMenuListMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery =
                DynamicWrapperExtensions.ToDynamic(
                    QueryMetadata.GetConfigurationMetadataShortListIql(target.Item.ConfigId, target.Item.MetadataType));

            dynamic response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            if (queryResult.Any())
            {
                target.Result = queryResult.First().Result.Menu;
            }
            else
            {
                target.Result = new List<dynamic>();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetMenuListMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery = DynamicWrapperExtensions.ToDynamic((string)QueryMetadata.GetConfigurationMetadataShortListIql(target.Item.ConfigId, target.Item.MetadataType));

            var response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
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

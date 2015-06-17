using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetDocumentElementListMetadata
    {
        public void Action(IApplyResultContext target)
        {
            IMetadataContainerInfo containerInfo = new MetadataContainerInfoFactory().BuildMetadataContainerInfo(target.Item.MetadataType);

            dynamic bodyQuery = QueryMetadata.GetDocumentMetadataShortListIql(target.Item.ConfigId, target.Item.DocumentId,containerInfo.GetMetadataContainerName()).ToDynamic();

            var response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery, target.Version);
            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            target.Result = 
                queryResult.Any() ? 
                queryResult.First().Result[containerInfo.GetMetadataContainerName()] : 
                new List<dynamic>();
        }
    }
}

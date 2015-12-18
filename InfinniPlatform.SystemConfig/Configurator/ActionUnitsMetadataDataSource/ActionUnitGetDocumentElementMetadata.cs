using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetDocumentElementMetadata
    {
        public ActionUnitGetDocumentElementMetadata(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyResultContext target)
        {
            IMetadataContainerInfo containerInfo =
                new MetadataContainerInfoFactory().BuildMetadataContainerInfo(target.Item.MetadataType);

            dynamic bodyQuery =
                QueryMetadata.GetDocumentMetadataByNameIql(target.Item.ConfigId, target.Item.DocumentId,
                    target.Item.MetadataName,
                    containerInfo.GetMetadataContainerName(),
                    containerInfo.GetMetadataTypeName()).ToDynamic();

            dynamic response = _restQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);

            if (queryResult.Any())
            {
                dynamic container = queryResult.First().Result[containerInfo.GetMetadataContainerName()];
                if (container != null && container.Count > 0)
                {
                    target.Result = container[0][string.Format("{0}Full", containerInfo.GetMetadataTypeName())];
                    return;
                }
            }
            target.Result = null;
        }
    }
}
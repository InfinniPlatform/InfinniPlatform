using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetDocumentMetadata
    {
        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery = DynamicWrapperExtensions.ToDynamic((string)QueryMetadata.GetConfigurationMetadataByNameIql(target.Item.ConfigId, target.Item.DocumentId, new MetadataContainerDocument().GetMetadataContainerName(), new MetadataContainerDocument().GetMetadataTypeName()));


            var response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null, bodyQuery);
            IEnumerable<dynamic> queryResult = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
            
            IEnumerable<dynamic> documents = new List<dynamic>();
            if (queryResult.Any())
            {
                documents = queryResult.First().Result.Documents;
            }

            target.Result = documents != null ? documents.Select(d => d.DocumentFull).FirstOrDefault() : null;
        }
    }
}

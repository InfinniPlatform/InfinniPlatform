using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    public sealed class ActionUnitGetValidationErrorMetadata
    {
        public ActionUnitGetValidationErrorMetadata(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyResultContext target)
        {
            dynamic bodyQuery = new DynamicWrapper();
            bodyQuery.ConfigId = target.Item.ConfigId;
            bodyQuery.DocumentId = target.Item.DocumentId;
            bodyQuery.MetadataType = MetadataType.ValidationError;
            bodyQuery.MetadataName = target.Item.MetadataName;

            target.Result = _restQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getdocumentelementmetadata", null, bodyQuery).ToDynamic();
        }
    }
}
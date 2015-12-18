using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    /// Получить метаданные загруженной конфигурации сервера
    /// </summary>
    public sealed class ActionUnitGetConfigMetadata
    {
        public ActionUnitGetConfigMetadata(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyContext target)
        {
            var paramsDoc = target.Item;
            if (target.Item.Document != null)
            {
                paramsDoc = target.Item.Document;
            }


            //если указано конкретное наименование искомых метаданных
            target.Result = target.Context.GetComponent<IMetadataComponent>()
                                  .GetMetadata(paramsDoc.Configuration, paramsDoc.Metadata, paramsDoc.MetadataType, paramsDoc.MetadataName);

            if (target.Result != null)
            {
                var service = target.Context.GetComponent<IMetadataComponent>()
                                    .GetMetadata(paramsDoc.Configuration, "Common", MetadataType.Service, "FilterMetadata");

                if (service != null)
                {
                    target.Result.Version = null;

                    dynamic filterMetadata = _restQueryApi.QueryPostJsonRaw(paramsDoc.Configuration, "Common", "FilterMetadata", null, target.Result).ToDynamic();

                    if (filterMetadata != null)
                    {
                        target.Result = filterMetadata;
                    }
                }
            }
        }
    }
}
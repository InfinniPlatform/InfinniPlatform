using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    ///     Получить метаданные загруженной конфигурации сервера
    /// </summary>
    public sealed class ActionUnitGetConfigMetadata
    {
        public void Action(IApplyContext target)
        {
            var paramsDoc = target.Item;
            if (target.Item.Document != null)
            {
                paramsDoc = target.Item.Document;
            }


            var authUtils = new AuthUtils(target.Context.GetComponent<ISecurityComponent>(),
                                          target.UserName, null);

            //если указано конкретное наименование искомых метаданных
            target.Result = target.Context.GetComponent<IMetadataComponent>()
                                  .GetMetadata(target.Item.Version, paramsDoc.Configuration, paramsDoc.Metadata,
                                               paramsDoc.MetadataType, paramsDoc.MetadataName);

            if (target.Result != null)
            {
                ValidationResult validationResult = authUtils.CheckDocumentAccess("SystemConfig",
                                                                                  paramsDoc.MetadataType
                                                                                           .ToLowerInvariant() +
                                                                                  "metadata", "getdocument",
                                                                                  target.Result.Id);
                if (!validationResult.IsValid)
                {
                    target.Result = null;
                }
            }

            if (target.Result != null)
            {
                var service = target.Context.GetComponent<IMetadataComponent>()
                                    .GetMetadata(null, paramsDoc.Configuration, "Common", MetadataType.Service,
                                                 "FilterMetadata");

                if (service != null)
                {
                    target.Result.Version = null;

                    dynamic filterMetadata =
                        RestQueryApi.QueryPostJsonRaw(paramsDoc.Configuration, "Common", "FilterMetadata", null,
                                                      target.Result).ToDynamic();

                    if (filterMetadata != null)
                    {
                        target.Result = filterMetadata;
                    }
                }
            }
        }
    }
}
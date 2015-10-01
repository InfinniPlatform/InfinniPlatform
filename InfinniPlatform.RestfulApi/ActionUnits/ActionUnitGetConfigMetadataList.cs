using System.Collections.Generic;
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
    public sealed class ActionUnitGetConfigMetadataList
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


            target.Result = target.Context.GetComponent<IMetadataComponent>()
                                  .GetMetadataList(target.Item.Version, paramsDoc.Configuration, paramsDoc.Metadata,
                                                   paramsDoc.MetadataType);


            var result = new List<dynamic>();
            foreach (dynamic o in target.Result)
            {
                if (o == null)
                {
                    continue;                    
                }
                ValidationResult validationResult = authUtils.CheckDocumentAccess("SystemConfig",
                                                                                  paramsDoc.MetadataType + "metadata",
                                                                                  "getdocument",
                                                                                  o.Id);
                if (validationResult.IsValid)
                {
                    result.Add(o);
                }
            }

            target.Result = result;
        }
    }
}
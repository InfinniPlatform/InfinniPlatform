using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль фильтрации документов, на которые у пользователя есть доступ
    /// </summary>
    public sealed class ActionUnitFilterAuthDocument
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Secured == null || target.Item.Secured == true)
            {
                var result = DynamicWrapperExtensions.ToEnumerable(target.Result);

                var resultChecked = new List<dynamic>();

                foreach (dynamic document in result)
                {
                    dynamic validationResult =
                        new AuthUtils(target.Context.GetComponent<ISecurityComponent>(target.Version), target.UserName,
                                      null).CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata,
                                                                "getdocument", document.Id);

                    if (validationResult.IsValid)
                    {
                        resultChecked.Add(document);
                    }
                }
                target.Result = resultChecked;
            }
        }
    }
}
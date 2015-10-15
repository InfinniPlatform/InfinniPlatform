using System;
using System.Linq;

using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль проверки доступа к действию администрирования указанного объекта
    /// </summary>
    public sealed class ActionUnitAdminAuth
    {
        public void Action(IApplyContext target)
        {
            ValidationResult authResult =
                new AuthUtils(target.Context.GetComponent<ISecurityComponent>(), target.UserName, null)
                    .CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata, "applyaccess",
                                         target.Item.RecordId);

            target.IsValid = authResult.IsValid;
			target.ValidationMessage = string.Join(Environment.NewLine, (authResult.Items != null) ? authResult.Items.Select(i => i.ToString()) : Enumerable.Empty<string>());
        }
    }
}
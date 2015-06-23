using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Contracts;

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
                new AuthUtils(target.Context.GetComponent<ISecurityComponent>(target.Version), target.UserName, null)
                    .CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata, "applyaccess",
                                         target.Item.RecordId);

            target.IsValid = authResult.IsValid;
            target.ValidationMessage = string.Join("\r\n", authResult.Items);
        }
    }
}
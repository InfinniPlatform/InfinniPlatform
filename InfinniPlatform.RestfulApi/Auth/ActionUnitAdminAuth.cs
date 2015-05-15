using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль проверки доступа к действию администрирования указанного объекта
    /// </summary>
    public sealed class ActionUnitAdminAuth
    {
        public void Action(IApplyContext target)
        {
            var authResult = new AuthUtils(target.Context.GetComponent<ISecurityComponent>(),target.UserName, null).CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata, "applyaccess",target.Item.RecordId);

            target.IsValid = authResult.IsValid;
            target.ValidationMessage = string.Join("\r\n",authResult.Items);
        }
    }
}

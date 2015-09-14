using System;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Administration.MenuPermission
{
    public sealed class ActionUnitDeleteMenuPermission
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Document.Menu == null)
            {
                target.IsValid = false;
                target.ValidationMessage = "Menu to delete permission should not be empty.";
                return;
            }

            var documentApi = target.Context.GetComponent<DocumentApi>();

            var aclApi = target.Context.GetComponent<AuthApi>();

            try
            {
                //запрещаем доступ пользователю
                aclApi.DenyAccess(target.Item.Document.Role.DisplayName, "SystemConfig", "MenuMetadata", "getdocument",
                    target.Item.Document.Menu.Id);
            }
            catch (Exception)
            {
                target.IsValid = false;
                target.ValidationMessage = string.Format("User {0} access denied to change user access. ",
                    target.UserName);
                return;
            }

            documentApi.DeleteDocument("Administration", "MenuPermission", target.Item.Document.Id);
        }
    }
}
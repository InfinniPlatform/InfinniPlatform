using System;

using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.RestApi.DataApi;
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
                var userName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;

                target.IsValid = false;
                target.ValidationMessage = $"User {userName} access denied to change user access. ";
                return;
            }

            documentApi.DeleteDocument("Administration", "MenuPermission", target.Item.Document.Id);
        }
    }
}
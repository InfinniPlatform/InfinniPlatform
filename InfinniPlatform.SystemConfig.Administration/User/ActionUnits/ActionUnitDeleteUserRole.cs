using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitDeleteUserRole
    {
        public void Action(IApplyContext target)
        {
            dynamic item = target.Item.Document ?? target.Item;

            var api = target.Context.GetComponent<ISecurityComponent>();

            dynamic user = api.Users.FirstOrDefault(u => u.UserName == item.UserName);

            //добавляем роль в конфигурации Authorization
            var authApi = target.Context.GetComponent<AuthApi>();

            authApi.RemoveUserRole(item.UserName, item.RoleName);

            if (user != null)
            {
                IEnumerable<dynamic> userRoles = DynamicWrapperExtensions.ToEnumerable(user.UserRoles);
                user.UserRoles = userRoles.Where(r => r.DisplayName != item.RoleName).ToList();
                target.Context.GetComponent<DocumentApi>().SetDocument("Administration", "User", user);
            }

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "User role deleted.";
        }
    }
}
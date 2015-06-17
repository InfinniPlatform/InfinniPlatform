using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitAddUserRole
    {
        public void Action(IApplyContext target)
        {
            dynamic item = target.Item.Document ?? target.Item;

            var api = target.Context.GetComponent<ISecurityComponent>(target.Version);

            dynamic user = api.Users.FirstOrDefault(u => u.UserName == item.UserName);

            if (user != null)
            {

                //добавляем роль в конфигурации Authorization
                var authApi = target.Context.GetComponent<AuthApi>(target.Version);

                authApi.AddUserToRole(item.UserName, item.RoleName);

                dynamic documentRole = new DynamicWrapper();
                documentRole.Id = Guid.NewGuid().ToString();
                documentRole.DisplayName = item.RoleName;

                user.UserRoles = user.UserRoles ?? new List<dynamic>();
                user.UserRoles.Add(documentRole);

                target.Context.GetComponent<DocumentApi>(target.Version).SetDocument("Administration", "User", user);

                target.Result = new DynamicWrapper();
                target.Result.IsValid = true;
                target.Result.ValidationMessage = "User role added.";
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = "User not found.";
            }
        }
    }
}

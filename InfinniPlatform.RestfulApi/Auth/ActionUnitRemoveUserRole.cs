using System;
using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль удаления роли у пользователя
    /// </summary>
    public sealed class ActionUnitRemoveUserRole
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            ApplicationUser user = storage.FindUserByName(target.Item.UserName);
            if (user == null)
            {
                throw new ArgumentException(string.Format("User {0} not found", target.Item.UserName));
            }

            storage.RemoveUserFromRole(user, target.Item.RoleName);
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateAcl();
            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role deleted.";
        }
    }
}
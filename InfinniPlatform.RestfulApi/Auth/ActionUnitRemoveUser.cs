using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль удаления пользователей системы
    /// </summary>
    public sealed class ActionUnitRemoveUser
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            ApplicationUser user = storage.FindUserByName(target.Item.UserName);
            if (user != null)
            {
                storage.DeleteUser(user);
                //добавляем доступ на чтение пользователей
                target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateUsers();
                target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateAcl();
                target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateUserRoles();
                target.Result = new DynamicWrapper();
                target.Result.IsValid = true;
                target.Result.ValidationMessage = "User deleted";
            }
        }
    }
}
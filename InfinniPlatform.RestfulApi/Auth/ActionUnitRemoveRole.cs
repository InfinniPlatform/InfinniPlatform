using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль удаления роли пользователя
    /// </summary>
    public sealed class ActionUnitRemoveRole
    {
        public void Action(IApplyContext target)
        {
            var storage = new ApplicationUserStorePersistentStorage();
            storage.RemoveRole(target.Item.RoleName);
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateAcl();
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateRoles();
            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role removed.";
        }
    }
}
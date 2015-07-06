using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
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
            target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
            target.Context.GetComponent<CachedSecurityComponent>().UpdateRoles();
            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role removed.";
        }
    }
}
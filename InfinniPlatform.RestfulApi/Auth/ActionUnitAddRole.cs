using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль добавления роли
    /// </summary>
    public sealed class ActionUnitAddRole
    {
        public void Action(IApplyContext target)
        {
            var storage = ApplicationUserStorePersistentStorage.Instance;

            dynamic roleParams = target.Item;
            if (target.Item.Document != null)
            {
                roleParams = target.Item.Document;
            }

            storage.AddRole(roleParams.Name, roleParams.Caption, roleParams.Description);
            target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
            target.Context.GetComponent<CachedSecurityComponent>().UpdateRoles();
            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.ValidationMessage = "Role added.";
        }
    }
}
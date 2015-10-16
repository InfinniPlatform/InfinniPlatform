using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Удалить ACL
    /// </summary>
    public sealed class ActionUnitRemoveAcl
    {
        public void Action(IApplyContext target)
        {
            var storage = ApplicationUserStorePersistentStorage.Instance;
            storage.RemoveAcl(target.Item.AclId);
            target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
        }
    }
}
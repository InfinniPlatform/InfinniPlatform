using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;
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
            var storage = new ApplicationUserStorePersistentStorage();
            storage.RemoveAcl(target.Item.AclId);
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateAcl();
        }
    }
}
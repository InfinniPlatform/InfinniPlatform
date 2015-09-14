using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль обновления ACL в кэше сервера
    /// </summary>
    public sealed class ActionUnitUpdateAcl
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
        }
    }
}
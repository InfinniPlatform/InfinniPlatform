using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль вызова обновления данных о ролях пользователей в кэше
    /// </summary>
    public sealed class ActionUnitUpdateUserRoles
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<CachedSecurityComponent>(target.Version).UpdateUserRoles();
        }
    }
}
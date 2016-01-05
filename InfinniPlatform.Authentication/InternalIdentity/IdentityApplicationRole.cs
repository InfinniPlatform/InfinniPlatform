using InfinniPlatform.Core.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    /// <summary>
    /// Сведения о роли системы с реализацией интерфейса <see cref="IRole" />.
    /// </summary>
    internal sealed class IdentityApplicationRole : ApplicationRole, IRole
    {
    }
}
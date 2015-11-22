using InfinniPlatform.Api.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.Internaldentity
{
    /// <summary>
    /// Сведения о роли системы с реализацией интерфейса <see cref="IRole" />.
    /// </summary>
    internal sealed class IdentityApplicationRole : ApplicationRole, IRole
    {
    }
}
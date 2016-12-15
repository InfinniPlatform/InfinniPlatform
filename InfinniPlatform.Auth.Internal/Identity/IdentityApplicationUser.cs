using InfinniPlatform.Auth.Internal.Contract;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Сведения о пользователе системы с реализацией интерфейса <see cref="IUser" />.
    /// </summary>
    internal class IdentityApplicationUser : ApplicationUser, IUser
    {
    }
}
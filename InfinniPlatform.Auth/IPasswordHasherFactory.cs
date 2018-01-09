using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Factory for creating <see cref="IPasswordHasher{TUser}"/> instance.
    /// </summary>
    public interface IPasswordHasherFactory
    {
        /// <summary>
        /// Returns <see cref="IPasswordHasher{TUser}"/> instance.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <param name="resolver">Application container resolver.</param>
        IPasswordHasher<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
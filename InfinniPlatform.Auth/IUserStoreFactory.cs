using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Factory for creating <see cref="IUserStore{TUser}"/> instance.
    /// </summary>
    public interface IUserStoreFactory
    {
        /// <summary>
        /// Returns <see cref="IUserStore{TUser}"/> instances. 
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <param name="resolver">Application container resolver.</param>
        IUserStore<TUser> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
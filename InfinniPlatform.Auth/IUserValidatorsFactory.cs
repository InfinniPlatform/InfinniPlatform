using System.Collections.Generic;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Factory for creating <see cref="IUserValidator{TUser}"/> instances.
    /// </summary>
    public interface IUserValidatorsFactory
    {
        /// <summary>
        /// Returns list of <see cref="IUserValidator{TUser}"/> instances.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <param name="resolver">Application container resolver.</param>
        IEnumerable<IUserValidator<TUser>> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
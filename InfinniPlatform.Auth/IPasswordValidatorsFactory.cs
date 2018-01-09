using System.Collections.Generic;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Фабрика для получения валидаторов паролей <see cref="IPasswordValidator{TUser}"/>.
    /// </summary>
    public interface IPasswordValidatorsFactory
    {
        /// <summary>
        /// Returns <see cref="IPasswordValidator{TUser}"/> instances.
        /// </summary>
        /// <typeparam name="TUser">User type.</typeparam>
        /// <param name="resolver">Application container resolver.</param>
        IEnumerable<IPasswordValidator<TUser>> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
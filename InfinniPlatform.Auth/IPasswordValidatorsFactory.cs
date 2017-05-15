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
        IEnumerable<IPasswordValidator<TUser>> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
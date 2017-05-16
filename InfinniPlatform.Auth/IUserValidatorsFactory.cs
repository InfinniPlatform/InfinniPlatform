using System.Collections.Generic;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Фабрика для получения валидаторов пользователей <see cref="IUserValidator{TUser}"/>.
    /// </summary>
    public interface IUserValidatorsFactory
    {
        IEnumerable<IUserValidator<TUser>> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
using System.Collections.Generic;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// ‘абрика дл€ получени€ валидаторов пользователей <see cref="IUserValidator{TUser}"/>.
    /// </summary>
    public interface IUserValidatorsFactory
    {
        IEnumerable<IUserValidator<TUser>> Get<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
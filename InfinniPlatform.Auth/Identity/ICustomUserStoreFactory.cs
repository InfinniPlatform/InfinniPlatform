﻿using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    /// Фабрика для получения хранилища пользователей.
    /// </summary>
    public interface ICustomUserStoreFactory
    {
        IUserStore<TUser> GetUserStore<TUser>(IContainerResolver resolver) where TUser : AppUser;
    }
}
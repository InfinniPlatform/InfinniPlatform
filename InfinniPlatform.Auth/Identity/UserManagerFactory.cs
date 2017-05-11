using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Identity
{
    internal class UserManagerFactory : IUserManagerFactory
    {
        private readonly IContainerResolver _containerResolver;

        public UserManagerFactory(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }

        public UserManager<TUser> GetUserManager<TUser>() where TUser : AppUser
        {
            // Хранилище пользователей
            var appUserStoreFactory = _containerResolver.Resolve<IUserStoreFactory>();
            var appUserStore = appUserStoreFactory.GetUserStore<TUser>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher<TUser>();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<TUser>> {new AppUserValidator<TUser>(appUserStore)};

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<TUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = _containerResolver.Resolve<IServiceProvider>();

            // Логгер
            var logger = _containerResolver.Resolve<ILogger<UserManager<TUser>>>();

            var userManager = new UserManager<TUser>(appUserStore,
                                                     optionsAccessor,
                                                     identityPasswordHasher,
                                                     userValidators,
                                                     passwordValidators,
                                                     keyNormalizer,
                                                     identityErrorDescriber,
                                                     serviceProvider,
                                                     logger);

            return userManager;
        }
    }
}
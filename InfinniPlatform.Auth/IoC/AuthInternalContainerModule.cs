using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.DocumentStorage;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.Services;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.IoC
{
    public class AuthInternalContainerModule<TUser> : IContainerModule where TUser : AppUser
    {
        private readonly AuthOptions _options;

        public AuthInternalContainerModule(AuthOptions options)
        {
            _options = options;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // User storage

            builder.RegisterType<UserStore<TUser>>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(CreateUserStore)
                   .As<IUserStore<TUser>>()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<TUser>>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // UserStorage

            builder.RegisterType<UserCache<AppUser>>()
                   .As<IUserCacheObserver>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthInternalConsumerSource>()
                   .As<IConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AuthInternalDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();
        }


        private IUserStore<TUser> CreateUserStore(IContainerResolver resolver)
        {
            return _options.UserStoreFactory?.GetUserStore<TUser>(resolver) ?? resolver.Resolve<UserStore<TUser>>();
        }

        private static UserManager<TUser> CreateUserManager(IContainerResolver resolver)
        {
            // Хранилище пользователей
            var appUserStore = resolver.Resolve<IUserStore<TUser>>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher<TUser>();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<TUser>> { new AppUserValidator<TUser>(appUserStore) };

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<TUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = resolver.Resolve<IServiceProvider>();

            // Логгер
            var logger = resolver.Resolve<ILogger<UserManager<TUser>>>();

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
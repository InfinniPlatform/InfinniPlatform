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
    public class AuthInternalContainerModule : IContainerModule
    {
        private readonly AuthInternalOptions _options;

        public AuthInternalContainerModule(AuthInternalOptions options)
        {
            _options = options;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // User storage

            builder.RegisterType<UserStore<AppUser>>()
                   .As<IUserStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserAuthenticationTokenStore<AppUser>>()
                   .As<IUserAuthenticationTokenStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserClaimStore<AppUser>>()
                   .As<IUserClaimStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserEmailStore<AppUser>>()
                   .As<IUserEmailStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserLockoutStore<AppUser>>()
                   .As<IUserLockoutStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserLoginStore<AppUser>>()
                   .As<IUserLoginStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserPasswordStore<AppUser>>()
                   .As<IUserPasswordStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserPhoneNumberStore<AppUser>>()
                   .As<IUserPhoneNumberStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserRoleStore<AppUser>>()
                   .As<IUserRoleStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserSecurityStampStore<AppUser>>()
                   .As<IUserSecurityStampStore<AppUser>>()
                   .SingleInstance();

            builder.RegisterType<UserTwoFactorStore<AppUser>>()
                   .As<IUserTwoFactorStore<AppUser>>()
                   .SingleInstance();

            // Role storage

            builder.RegisterType<RoleStore<AppUserRole>>()
                   .As<IRoleStore<AppUserRole>>()
                   .SingleInstance();

            // User manager

            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<AppUser>>()
                   .ExternallyOwned();

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

            builder.RegisterType<AuthInternalHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
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


        private static UserManager<AppUser> CreateUserManager(IContainerResolver resolver)
        {
            // Хранилище пользователей
            var appUserStore = resolver.Resolve<IUserStore<AppUser>>();
            var appUserEmailStore = resolver.Resolve<IUserEmailStore<AppUser>>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<AppUser>> {new AppUserValidator(appUserStore, appUserEmailStore)};

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<AppUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = resolver.Resolve<IServiceProvider>();

            // Логгер
            var logger = resolver.Resolve<ILogger<UserManager<AppUser>>>();

            var userManager = new UserManager<AppUser>(appUserStore,
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
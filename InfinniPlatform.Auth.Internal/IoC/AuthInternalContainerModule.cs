using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Identity.MongoDb;
using InfinniPlatform.Auth.Middlewares;
using InfinniPlatform.Auth.Services;
using InfinniPlatform.Auth.UserStorage;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.Settings;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.IoC
{
    public class AuthInternalContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // AspNet.Identity

            builder.RegisterFactory(CreateUserStore)
                   .As<UserStore<IdentityUser>>()
                   .As<IUserStore<IdentityUser>>()
                   .SingleInstance();

            builder.RegisterFactory(CreateRoleStore)
                   .As<RoleStore<IdentityRole>>()
                   .As<IRoleStore<IdentityRole>>()
                   .SingleInstance();

            // Менеджер работы с учетными записями пользователей для AspNet.Identity
            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<IdentityUser>>()
                   .ExternallyOwned();

            // Middlewares

            builder.RegisterType<AuthInternalHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthInternalHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();

            // UserStorage

            builder.RegisterFactory(GetUserStorageSettings)
                   .As<UserStorageSettings>()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCache>()
                   .As<IUserCacheSynchronizer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthInternalMessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AuthInternalDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();

            builder.RegisterType<AppUserStore>()
                   .As<IAppUserStore>()
                   .SingleInstance();

            // Cookie

            builder.RegisterFactory(GetSettings)
                   .As<AuthCookieHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static UserManager<IdentityUser> CreateUserManager(IContainerResolver resolver)
        {
            // Хранилище учетных записей пользователей для AspNet.Identity
            var identityUserStore = resolver.Resolve<UserStore<IdentityUser>>();

            // Провайдер настроек AspNet.Identity
            var optionsAccessor = new OptionsWrapper<IdentityOptions>(new IdentityOptions());

            // Сервис хэширования паролей
            var identityPasswordHasher = new DefaultAppUserPasswordHasher();

            // Валидаторы данных о пользователях
            var userValidators = new List<IUserValidator<IdentityUser>> {new IdentityApplicationUserValidator(identityUserStore)};

            // Валидатор паролей пользователей
            var passwordValidators = Enumerable.Empty<IPasswordValidator<IdentityUser>>();

            // Нормализатор
            var keyNormalizer = new UpperInvariantLookupNormalizer();

            // Сервис обработки ошибок AspNet.Identity
            var identityErrorDescriber = new IdentityErrorDescriber();

            // Провайдер зарегистрированных в IoC сервисов
            var serviceProvider = resolver.Resolve<IServiceProvider>();

            // Логгер
            var logger = resolver.Resolve<ILogger<UserManager<IdentityUser>>>();

            var userManager = new UserManager<IdentityUser>(identityUserStore,
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

        private static UserStorageSettings GetUserStorageSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName);
        }

        private static UserStore<IdentityUser> CreateUserStore(IContainerResolver resolver)
        {
            var documentStorage = resolver.Resolve<ISystemDocumentStorage<IdentityUser>>();

            return new UserStore<IdentityUser>(documentStorage);
        }

        private static RoleStore<IdentityRole> CreateRoleStore(IContainerResolver resolver)
        {
            var documentStorage = resolver.Resolve<IDocumentStorage<IdentityRole>>();

            return new RoleStore<IdentityRole>(documentStorage);
        }

        private static AuthCookieHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthCookieHttpMiddlewareSettings>(AuthCookieHttpMiddlewareSettings.SectionName);
        }
    }
}
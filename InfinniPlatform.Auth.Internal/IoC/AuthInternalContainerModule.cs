using System;
using System.Collections.Generic;
using InfinniPlatform.Auth.Internal.Identity;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
using InfinniPlatform.Auth.Internal.Middlewares;
using InfinniPlatform.Auth.Internal.Services;
using InfinniPlatform.Auth.Internal.UserStorage;
using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.Sdk.Metadata;
using InfinniPlatform.Sdk.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Internal.IoC
{
    internal class AuthInternalContainerModule : IContainerModule
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
        }


        private static UserManager<IdentityUser> CreateUserManager(IContainerResolver resolver)
        {
//            var appUserStore = resolver.Resolve<IAppUserStore>();
//            var appPasswordHasher = resolver.Resolve<IAppUserPasswordHasher>();

            // Хранилище учетных записей пользователей для AspNet.Identity
            var identityUserStore = resolver.Resolve<UserStore<IdentityUser>>();

            // Сервис проверки учетных записей пользователей для AspNet.Identity
            var identityUserValidator = new IdentityApplicationUserValidator(identityUserStore);

            // Сервис хэширования паролей пользователей для AspNet.Identity
            var identityPasswordHasher = new DefaultAppUserPasswordHasher();

            // TODO Refactor validators.

            var userManager = new UserManager<IdentityUser>(identityUserStore,
                                                            new OptionsWrapper<IdentityOptions>(new IdentityOptions()),
                                                            identityPasswordHasher,
                                                            new List<IUserValidator<IdentityUser>> {identityUserValidator},
                                                            new List<IPasswordValidator<IdentityUser>>(),
                                                            new UpperInvariantLookupNormalizer(),
                                                            new IdentityErrorDescriber(),
                                                            resolver.Resolve<IServiceProvider>(),
                                                            null);

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
    }
}
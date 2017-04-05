using InfinniPlatform.Auth.Internal.Identity;
using InfinniPlatform.Auth.Internal.Middlewares;
using InfinniPlatform.Auth.Internal.Services;
using InfinniPlatform.Auth.Internal.UserStorage;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Documents.Metadata;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Settings;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace InfinniPlatform.Auth.Internal.IoC
{
    internal class AuthInternalContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // AspNet.Identity

            // Менеджер работы с учетными записями пользователей для AspNet.Identity
            builder.RegisterFactory(CreateUserManager)
                   .As<UserManager<IdentityApplicationUser>>()
                   .ExternallyOwned();

            // Сервис хэширования паролей пользователей на уровне приложения
            builder.RegisterType<DefaultAppUserPasswordHasher>()
                   .As<IAppUserPasswordHasher>()
                   .SingleInstance();

            // Менеджер работы с учетными записями пользователей на уровне приложения
            builder.RegisterType<IdentityAppUserManager>()
                   .As<IAppUserManager>()
                   .SingleInstance();

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


        private static UserManager<IdentityApplicationUser> CreateUserManager(IContainerResolver resolver)
        {
            var appUserStore = resolver.Resolve<IAppUserStore>();
            var appPasswordHasher = resolver.Resolve<IAppUserPasswordHasher>();

            // Хранилище учетных записей пользователей для AspNet.Identity
            var identityUserStore = new IdentityApplicationUserStore(appUserStore);

            // Сервис проверки учетных записей пользователей для AspNet.Identity
            var identityUserValidator = new IdentityApplicationUserValidator(identityUserStore);

            // Сервис хэширования паролей пользователей для AspNet.Identity
            var identityPasswordHasher = new IdentityApplicationUserPasswordHasher(appPasswordHasher);

            // Сервис генерации токенов безопасности для подтверждения действий
            var dataProtectionProvider = new DpapiDataProtectionProvider("InfinniPlatform");
            var dataProtector = dataProtectionProvider.Create("EmailConfirmation");
            var tokenProvider = new DataProtectorTokenProvider<IdentityApplicationUser>(dataProtector);

            var userManager = new UserManager<IdentityApplicationUser>(identityUserStore)
                              {
                                  UserTokenProvider = tokenProvider,
                                  UserValidator = identityUserValidator,
                                  PasswordHasher = identityPasswordHasher
                              };

            return userManager;
        }

        private static UserStorageSettings GetUserStorageSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName);
        }
    }
}
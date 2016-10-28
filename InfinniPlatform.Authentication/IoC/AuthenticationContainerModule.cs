using InfinniPlatform.Authentication.Hosting;
using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Authentication.RazorViews;
using InfinniPlatform.Authentication.Security;
using InfinniPlatform.Authentication.Services;
using InfinniPlatform.Authentication.UserStorage;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Metadata.Documents;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.IoC
{
    internal sealed class AuthenticationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
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

            // Идентификационные данных текущего пользователя
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();

            // Модули аутентификации

            builder.RegisterType<CookieAuthOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthAdfsOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthFacebookOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthGoogleOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ExternalAuthVkOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<InternalAuthOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<UserStorageSettings>(UserStorageSettings.SectionName))
                   .As<UserStorageSettings>()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCache>()
                   .AsSelf()
                   .As<IUserCacheSynchronizer>()
                   .SingleInstance();

            builder.RegisterType<AppUserStoreCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthenticationMessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<AppUserStore>()
                   .As<IAppUserStore>()
                   .SingleInstance();

            // Зависимости сервиса аутентификации

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<AuthHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();

            // Документы для хранение данных пользователя

            builder.RegisterType<AuthenticationDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();

            builder.RegisterType<RazorViewsHttpService>()
                   .As<IHttpService>()
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

            var userManager = new UserManager<IdentityApplicationUser>(identityUserStore)
            {
                UserValidator = identityUserValidator,
                PasswordHasher = identityPasswordHasher
            };

            return userManager;
        }
    }
}
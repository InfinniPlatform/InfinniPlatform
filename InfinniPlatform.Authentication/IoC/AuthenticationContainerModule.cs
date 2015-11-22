using InfinniPlatform.Api.Security;
using InfinniPlatform.Authentication.Internaldentity;
using InfinniPlatform.Authentication.Modules;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.IoC
{
    internal sealed class AuthenticationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Хранилище учетных записей пользователей для AspNet.Identity
            builder.RegisterType<IdentityApplicationUserStore>()
                   .As<IUserStore<IdentityApplicationUser>>()
                   .SingleInstance();

            // Сервис проверки учетных записей пользователей для AspNet.Identity
            builder.RegisterType<IdentityApplicationUserValidator>()
                   .As<IIdentityValidator<IdentityApplicationUser>>()
                   .SingleInstance();

            // Сервис хэширования паролей пользователей для AspNet.Identity
            builder.RegisterType<IdentityApplicationUserPasswordHasher>()
                   .As<IPasswordHasher>()
                   .SingleInstance();

            // Менеджер работы с учетными записями пользователей для AspNet.Identity
            builder.RegisterType<UserManager<IdentityApplicationUser>>()
                   .AsSelf()
                   .SingleInstance();

            // Сервис хэширования паролей пользователей на уровне приложения
            builder.RegisterType<DefaultApplicationUserPasswordHasher>()
                   .As<IApplicationUserPasswordHasher>()
                   .SingleInstance();

            // Менеджер работы с учетными записями пользователей на уровне приложения
            builder.RegisterType<IdentityApplicationUserManager>()
                   .As<IApplicationUserManager>()
                   .SingleInstance();

            // Модули аутентификации

            builder.RegisterType<CookieAuthOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<ExternalAuthAdfsOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<ExternalAuthEsiaOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<ExternalAuthFacebookOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<ExternalAuthGoogleOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<ExternalAuthVkOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();

            builder.RegisterType<InternalAuthOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();
        }
    }
}
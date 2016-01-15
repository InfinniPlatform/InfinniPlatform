using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Security;
using InfinniPlatform.Owin.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Owin.IoC
{
    internal sealed class OwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Сервис получения идентификационных данных текущего пользователя
            builder.RegisterType<OwinUserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<SystemInfoOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<SystemInfoOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ApplicationOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();
        }
    }
}
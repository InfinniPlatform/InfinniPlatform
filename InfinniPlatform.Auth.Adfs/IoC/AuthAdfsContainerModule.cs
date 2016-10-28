using InfinniPlatform.Auth.Adfs.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Auth.Adfs.IoC
{
    internal class AuthAdfsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetSettings)
                   .As<AuthAdfsHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthAdfsHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static AuthAdfsHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthAdfsHttpMiddlewareSettings>(AuthAdfsHttpMiddlewareSettings.SectionName);
        }
    }
}
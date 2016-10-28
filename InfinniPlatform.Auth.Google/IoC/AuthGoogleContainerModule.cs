using InfinniPlatform.Auth.Google.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Auth.Google.IoC
{
    internal class AuthGoogleContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetSettings)
                   .As<AuthGoogleHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthGoogleHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static AuthGoogleHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthGoogleHttpMiddlewareSettings>(AuthGoogleHttpMiddlewareSettings.SectionName);
        }
    }
}
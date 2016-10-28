using InfinniPlatform.Auth.Facebook.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Auth.Facebook.IoC
{
    internal class AuthFacebookContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetSettings)
                   .As<AuthFacebookHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthFacebookHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static AuthFacebookHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthFacebookHttpMiddlewareSettings>(AuthFacebookHttpMiddlewareSettings.SectionName);
        }
    }
}
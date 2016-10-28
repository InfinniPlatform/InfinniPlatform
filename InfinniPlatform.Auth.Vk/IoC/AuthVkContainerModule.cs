using InfinniPlatform.Auth.Vk.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Auth.Vk.IoC
{
    internal class AuthVkContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetSettings)
                   .As<AuthVkHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthVkHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static AuthVkHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthVkHttpMiddlewareSettings>(AuthVkHttpMiddlewareSettings.SectionName);
        }
    }
}
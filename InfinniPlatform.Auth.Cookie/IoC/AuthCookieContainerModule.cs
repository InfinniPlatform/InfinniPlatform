﻿using InfinniPlatform.Auth.Cookie.Middlewares;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Auth.Cookie.IoC
{
    internal class AuthCookieContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetSettings)
                   .As<AuthCookieHttpMiddlewareSettings>()
                   .SingleInstance();

            builder.RegisterType<AuthCookieHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();
        }


        private static AuthCookieHttpMiddlewareSettings GetSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<AuthCookieHttpMiddlewareSettings>(AuthCookieHttpMiddlewareSettings.SectionName);
        }
    }
}
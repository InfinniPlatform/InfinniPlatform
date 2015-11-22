using System;

using InfinniPlatform.Authentication.DataProtectors;
using InfinniPlatform.Authentication.Middleware;
using InfinniPlatform.Owin.Modules;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов аутентификации через Cookie.
    /// </summary>
    internal sealed class CookieAuthOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.CookieAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            if (IsRunningOnMono())
            {
                builder.SetDataProtectionProvider(new AesDataProtectionProvider());
            }

            // Разрешение использования cookie для входа в систему через внутренний провайдер

            var cookieAuthOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = InternalAuthOwinMiddleware.SignInInternalPath,
                LogoutPath = InternalAuthOwinMiddleware.SignOutPath,
                ExpireTimeSpan = TimeSpan.FromDays(1),
                SlidingExpiration = true
            };

            if (Uri.UriSchemeHttps.Equals(context.Configuration.ServerScheme, StringComparison.OrdinalIgnoreCase))
            {
                cookieAuthOptions.CookieSecure = CookieSecureOption.Always;
            }

            builder.UseCookieAuthentication(cookieAuthOptions);

            // Разрешение использования cookie для входа в систему через внешние провайдеры
            builder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }


        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
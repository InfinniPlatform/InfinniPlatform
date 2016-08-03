using System;

using InfinniPlatform.Authentication.DataProtectors;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
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
        public CookieAuthOwinHostingModule(IAppConfiguration appConfiguration)
        {
            _settings = appConfiguration.GetSection<CookieAuthOwinHostingModuleSettings>(CookieAuthOwinHostingModuleSettings.SectionName);
        }


        private readonly CookieAuthOwinHostingModuleSettings _settings;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.CookieAuth;


        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            // Домен для создания cookie
            var cookieDomain = _settings.CookieDomain;

            // Шифрование данных по умолчанию (работает также в Linux/Mono)
            builder.SetDataProtectionProvider(new AesDataProtectionProvider());

            // Разрешение использования cookie для входа в систему через внутренний провайдер

            var cookieAuthOptions = new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Auth/SignInInternal"),
                LogoutPath = new PathString("/Auth/SignOut"),
                ExpireTimeSpan = TimeSpan.FromDays(1),
                SlidingExpiration = true
            };

            if (!string.IsNullOrWhiteSpace(cookieDomain))
            {
                cookieAuthOptions.CookieDomain = cookieDomain;
            }

            if (Uri.UriSchemeHttps.Equals(context.Configuration.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                cookieAuthOptions.CookieSecure = CookieSecureOption.Always;
            }

            builder.UseCookieAuthentication(cookieAuthOptions);

            // Разрешение использования cookie для входа в систему через внешние провайдеры
            builder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
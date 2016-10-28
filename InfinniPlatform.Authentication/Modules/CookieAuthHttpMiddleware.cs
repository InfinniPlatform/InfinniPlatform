using System;

using InfinniPlatform.Authentication.DataProtectors;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http;
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
    internal sealed class CookieAuthHttpMiddleware : HttpMiddleware
    {
        public CookieAuthHttpMiddleware(IAppConfiguration appConfig, HostingConfig hostingConfig) : base(HttpMiddlewareType.AuthenticationBarrier)
        {
            _hostingConfig = hostingConfig;
            _settings = appConfig.GetSection<CookieAuthOwinHostingModuleSettings>(CookieAuthOwinHostingModuleSettings.SectionName);
        }


        private readonly CookieAuthOwinHostingModuleSettings _settings;
        private readonly HostingConfig _hostingConfig;


        public override void Configure(IAppBuilder builder)
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

            if (Uri.UriSchemeHttps.Equals(_hostingConfig.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                cookieAuthOptions.CookieSecure = CookieSecureOption.Always;
            }

            builder.UseCookieAuthentication(cookieAuthOptions);

            // Разрешение использования cookie для входа в систему через внешние провайдеры
            builder.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
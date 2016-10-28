using System;

using InfinniPlatform.Auth.Cookie.DataProtectors;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http;

using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;

using Owin;

namespace InfinniPlatform.Auth.Cookie.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Cookie.
    /// </summary>
    internal class AuthCookieHttpMiddleware : HttpMiddleware
    {
        public AuthCookieHttpMiddleware(AuthCookieHttpMiddlewareSettings settings, HostingConfig hostingConfig) : base(HttpMiddlewareType.AuthenticationBarrier)
        {
            _settings = settings;
            _hostingConfig = hostingConfig;
        }


        private readonly AuthCookieHttpMiddlewareSettings _settings;
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
                                        LoginPath = new PathString(_settings.LoginPath),
                                        LogoutPath = new PathString(_settings.LogoutPath),
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
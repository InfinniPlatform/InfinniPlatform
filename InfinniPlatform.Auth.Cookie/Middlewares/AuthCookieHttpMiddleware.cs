using System;
using InfinniPlatform.Auth.Cookie.DataProtectors;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
        private const string UriSchemeHttps = "https";


        public override void Configure(IApplicationBuilder builder)
        {
            //TODO
//            // Домен для создания cookie
//            var cookieDomain = _settings.CookieDomain;
//
//            // Разрешение использования cookie для входа в систему через внутренний провайдер
//            var cookieAuthOptions = new CookieAuthenticationOptions
//                                    {
//                                        AuthenticationScheme = "ApplicationCookie",
//                                        LoginPath = new PathString(_settings.LoginPath),
//                                        LogoutPath = new PathString(_settings.LogoutPath),
//                                        ExpireTimeSpan = TimeSpan.FromDays(1),
//                                        SlidingExpiration = true,
//                                        DataProtectionProvider = new AesDataProtectionProvider()
//                                    };
//
//            if (!string.IsNullOrWhiteSpace(cookieDomain))
//            {
//                cookieAuthOptions.CookieDomain = cookieDomain;
//            }
//
//            if (UriSchemeHttps.Equals(_hostingConfig.Scheme, StringComparison.OrdinalIgnoreCase))
//            {
//                cookieAuthOptions.CookieSecure = CookieSecurePolicy.Always;
//            }
//
//            builder.UseCookieAuthentication(cookieAuthOptions);
//
//            // Разрешение использования cookie для входа в систему через внешние провайдеры

//            //builder.UseExternalSignInCookie("ExternalCookie");
        }
    }
}
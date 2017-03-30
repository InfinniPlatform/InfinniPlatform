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
            // Разрешение использования cookie для входа в систему через внутренний провайдер
            var cookieAuthOptions = new CookieAuthenticationOptions
                                    {
                                        AuthenticationScheme = "MyCookieMiddlewareInstance",
                                        LoginPath = new PathString("/Account/Unauthorized/"),
                                        AccessDeniedPath = new PathString("/Account/Forbidden/"),
                                        AutomaticAuthenticate = true,
                                        AutomaticChallenge = true
                                    };

            builder.UseCookieAuthentication(cookieAuthOptions);
        }
    }
}
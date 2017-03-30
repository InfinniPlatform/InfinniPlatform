using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Auth.Cookie.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Cookie.
    /// </summary>
    internal class AuthCookieHttpMiddleware : HttpMiddleware
    {
        public AuthCookieHttpMiddleware() : base(HttpMiddlewareType.AuthenticationBarrier)
        {
        }
        
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
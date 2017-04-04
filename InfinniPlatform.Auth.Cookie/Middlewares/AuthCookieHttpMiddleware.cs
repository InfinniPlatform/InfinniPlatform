using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Cookie.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Cookie.
    /// </summary>
    internal class AuthCookieHttpMiddleware : HttpMiddleware
    {
        private readonly AuthCookieHttpMiddlewareSettings _settings;

        public AuthCookieHttpMiddleware(AuthCookieHttpMiddlewareSettings settings) : base(HttpMiddlewareType.AuthenticationBarrier)
        {
            _settings = settings;
        }

        public override void Configure(IApplicationBuilder builder)
        {
            // Разрешение использования cookie для входа в систему через внутренний провайдер
            var cookieOptions = new CookieAuthenticationOptions
                                {
                                    AuthenticationScheme = "Identity.Application",
                                    LoginPath = new PathString(_settings.LoginPath),
                                    LogoutPath = new PathString(_settings.LogoutPath),
                                    AutomaticAuthenticate = true,
                                    AutomaticChallenge = true
                                };

            builder.UseCookieAuthentication(cookieOptions);

            var externalCookieOptions = builder.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value.Cookies.ExternalCookie;

            externalCookieOptions.LoginPath = new PathString(_settings.LoginPath);
            externalCookieOptions.LogoutPath = new PathString(_settings.LogoutPath);
            externalCookieOptions.AutomaticAuthenticate = true;
            externalCookieOptions.AutomaticChallenge = true;

            builder.UseCookieAuthentication(externalCookieOptions);


        }
    }
}
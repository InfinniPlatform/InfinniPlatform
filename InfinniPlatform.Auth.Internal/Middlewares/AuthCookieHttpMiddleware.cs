using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Internal.Middlewares
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

        public override void Configure(IApplicationBuilder app)
        {
            var defaultCookiesOptions = app.ApplicationServices
                                               .GetRequiredService<IOptions<IdentityOptions>>()
                                               .Value
                                               .Cookies;

            var applicationCookieOptions = defaultCookiesOptions.ApplicationCookie;
            var externalCookieOptions = defaultCookiesOptions.ExternalCookie;

            applicationCookieOptions.LoginPath = new PathString(_settings.LoginPath);
            applicationCookieOptions.LogoutPath = new PathString(_settings.LogoutPath);
            applicationCookieOptions.AutomaticAuthenticate = true;
            applicationCookieOptions.AutomaticChallenge = true;

            externalCookieOptions.LoginPath = new PathString(_settings.LoginPath);
            externalCookieOptions.LogoutPath = new PathString(_settings.LogoutPath);
            externalCookieOptions.AutomaticAuthenticate = true;
            externalCookieOptions.AutomaticChallenge = true;

            app.UseCookieAuthentication(applicationCookieOptions);
            app.UseCookieAuthentication(defaultCookiesOptions.ExternalCookie);
        }
    }
}
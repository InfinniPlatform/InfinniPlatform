using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя через Cookie.
    /// </summary>
    internal class AuthCookieHttpMiddleware : HttpMiddlewareBase<AuthCookieMiddlewareOptions>
    {
        public AuthCookieHttpMiddleware(AuthOptions options) : base(HttpMiddlewareType.AuthenticationBarrier)
        {
            _options = options;
        }

        private readonly AuthOptions _options;

        public override void Configure(IApplicationBuilder app, AuthCookieMiddlewareOptions options)
        {
            var defaultCookiesOptions = app.ApplicationServices
                                           .GetRequiredService<IOptions<IdentityOptions>>()
                                           .Value
                                           .Cookies;

            var applicationCookieOptions = defaultCookiesOptions.ApplicationCookie;
            applicationCookieOptions.AutomaticAuthenticate = true;
            applicationCookieOptions.AutomaticChallenge = true;

            var externalCookieOptions = defaultCookiesOptions.ExternalCookie;
            externalCookieOptions.AutomaticAuthenticate = true;
            externalCookieOptions.AutomaticChallenge = true;

            app.UseCookieAuthentication(applicationCookieOptions);
            app.UseCookieAuthentication(defaultCookiesOptions.ExternalCookie);
        }
    }
}
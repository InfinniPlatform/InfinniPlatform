using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Hosting layer for user authentication via cookies.
    /// </summary>
    public class AuthCookieAppLayer : IAuthenticationBarrierAppLayer, IDefaultAppLayer
    {
        public AuthCookieAppLayer(AuthOptions options, IOptions<IdentityOptions> identityOptions)
        {
            _options = options;
            _identityOptions = identityOptions;
        }

        private readonly AuthOptions _options;
        private readonly IOptions<IdentityOptions> _identityOptions;

        public void Configure(IApplicationBuilder app)
        {
            var applicationCookieOptions = _identityOptions.Value.Cookies.ApplicationCookie;
            applicationCookieOptions.LoginPath = new PathString(_options.LoginPath);
            applicationCookieOptions.LogoutPath = new PathString(_options.LogoutPath);
            applicationCookieOptions.AutomaticAuthenticate = true;
            applicationCookieOptions.AutomaticChallenge = true;

            var externalCookieOptions = _identityOptions.Value.Cookies.ExternalCookie;
            externalCookieOptions.LoginPath = new PathString(_options.LoginPath);
            externalCookieOptions.LogoutPath = new PathString(_options.LogoutPath);
            externalCookieOptions.AutomaticAuthenticate = true;
            externalCookieOptions.AutomaticChallenge = true;

            app.UseCookieAuthentication(applicationCookieOptions);
            app.UseCookieAuthentication(externalCookieOptions);
        }
    }
}
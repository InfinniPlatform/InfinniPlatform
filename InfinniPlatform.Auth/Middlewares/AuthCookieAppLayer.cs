using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Hosting layer for user authentication via cookies.
    /// </summary>
    public class AuthCookieAppLayer : IAuthenticationBarrierAppLayer, IDefaultAppLayer
    {
        public AuthCookieAppLayer(IOptions<IdentityOptions> identityOptions)
        {
            _identityOptions = identityOptions;
        }

        private readonly IOptions<IdentityOptions> _identityOptions;

        public void Configure(IApplicationBuilder app)
        {
            //var applicationCookieOptions = _identityOptions.Value.Cookies.ApplicationCookie;
            //applicationCookieOptions.AutomaticAuthenticate = true;
            //applicationCookieOptions.AutomaticChallenge = true;

            //var externalCookieOptions = _identityOptions.Value.Cookies.ExternalCookie;
            //externalCookieOptions.AutomaticAuthenticate = true;
            //externalCookieOptions.AutomaticChallenge = true;

            //app.UseCookieAuthentication(applicationCookieOptions);
            //app.UseCookieAuthentication(externalCookieOptions);
        }
    }
}
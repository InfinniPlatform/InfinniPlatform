using System.Threading.Tasks;

using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Security;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Hosting layer for user authentication via internal methods (like database).
    /// </summary>
    public class AuthInternalAppLayer : IInternalAuthenticationAppLayer, IDefaultAppLayer
    {
        public AuthInternalAppLayer(IUserIdentityProvider identityProvider)
        {
            _identityProvider = identityProvider;
        }


        private readonly IUserIdentityProvider _identityProvider;


        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthInternalMiddleware>(this);
        }


        // ReSharper disable once ClassNeverInstantiated.Local
        private class AuthInternalMiddleware
        {
            public AuthInternalMiddleware(RequestDelegate next, AuthInternalAppLayer parentLayer)
            {
                _next = next;
                _parentLayer = parentLayer;
            }


            private readonly RequestDelegate _next;
            private readonly AuthInternalAppLayer _parentLayer;


            public async Task Invoke(HttpContext httpContext)
            {
                var requestUser = httpContext.User;

                _parentLayer._identityProvider.SetUserIdentity(requestUser);

                await _next.Invoke(httpContext);
            }
        }
    }
}
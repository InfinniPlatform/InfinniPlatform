using System.Threading.Tasks;

using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Logging;
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
        public AuthInternalAppLayer(IUserIdentityProvider identityProvider, ILog log)
        {
            _identityProvider = identityProvider;
            _log = log;
        }

        private readonly IUserIdentityProvider _identityProvider;
        private readonly ILog _log;

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AuthInternalMiddleware>(_identityProvider, _log);
        }


        private class AuthInternalMiddleware
        {
            public AuthInternalMiddleware(RequestDelegate next, IUserIdentityProvider identityProvider, ILog log)
            {
                _next = next;
                _identityProvider = identityProvider;
                _log = log;
            }

            private readonly RequestDelegate _next;
            private readonly IUserIdentityProvider _identityProvider;
            private readonly ILog _log;

            public async Task Invoke(HttpContext httpContext)
            {
                var requestUser = httpContext.User;

                _identityProvider.SetUserIdentity(requestUser);

                _log.SetUserId(requestUser?.Identity);

                await _next.Invoke(httpContext);
            }
        }
    }
}
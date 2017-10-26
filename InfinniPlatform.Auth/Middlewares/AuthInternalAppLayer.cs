using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Security;
using Microsoft.AspNetCore.Builder;

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
            app.Use((httpContext, next) =>
            {
                var requestUser = httpContext.User;

                _identityProvider.SetUserIdentity(requestUser);

                return next.Invoke();
            });
        }
    }
}
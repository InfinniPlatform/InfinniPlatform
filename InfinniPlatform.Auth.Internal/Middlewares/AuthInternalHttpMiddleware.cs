using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Logging;
using InfinniPlatform.Security;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Auth.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя на основе базы данных пользователей.
    /// </summary>
    internal class AuthInternalHttpMiddleware : HttpMiddlewareBase<AuthInternalMiddlewareOptions>
    {
        public AuthInternalHttpMiddleware(IUserIdentityProvider identityProvider, ILog log) : base(HttpMiddlewareType.InternalAuthentication)
        {
            _identityProvider = identityProvider;
            _log = log;
        }


        private readonly IUserIdentityProvider _identityProvider;
        private readonly ILog _log;


        public override void Configure(IApplicationBuilder app, AuthInternalMiddlewareOptions options)
        {
            // Прослойка для установки информации об идентификационных данных текущего пользователя
            app.Use((httpContext, nextOwinMiddleware) =>
                    {
                        var requestUser = httpContext.User;

                        _identityProvider.SetUserIdentity(requestUser);

                        _log.SetUserId(requestUser?.Identity);

                        return nextOwinMiddleware.Invoke();
                    });
        }
    }
}
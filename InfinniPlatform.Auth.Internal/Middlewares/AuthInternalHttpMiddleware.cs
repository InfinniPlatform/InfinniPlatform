using System;
using InfinniPlatform.Auth.Internal.Properties;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InfinniPlatform.Auth.Internal.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя на основе базы данных пользователей.
    /// </summary>
    internal class AuthInternalHttpMiddleware : HttpMiddleware
    {
        private readonly IUserIdentityProvider _identityProvider;
        private readonly ILog _log;

        public AuthInternalHttpMiddleware(IUserIdentityProvider identityProvider,
                                          ILog log)
            : base(HttpMiddlewareType.InternalAuthentication)
        {
            _identityProvider = identityProvider;
            _log = log;
        }


        public override void Configure(IApplicationBuilder builder)
        {
            // Прослойка для установки информации об идентификационных данных текущего пользователя
            builder.Use((httpContext, nextOwinMiddleware) =>
                        {
                            var requestUser = httpContext.User;

                            _identityProvider.SetUserIdentity(requestUser);

                            _log.SetUserId(requestUser?.Identity);

                            return nextOwinMiddleware.Invoke();
                        });
        }
    }
}
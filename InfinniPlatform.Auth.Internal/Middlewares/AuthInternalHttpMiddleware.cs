using System;

using InfinniPlatform.Auth.Internal.Contract;
using InfinniPlatform.Auth.Internal.Identity;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Internal.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя на основе базы данных пользователей.
    /// </summary>
    internal class AuthInternalHttpMiddleware : HttpMiddleware
    {
        public AuthInternalHttpMiddleware(Func<UserManager<IdentityUser>> userManagerFactory,
                                          IUserIdentityProvider identityProvider,
                                          ILog log)
            : base(HttpMiddlewareType.InternalAuthentication)
        {
            _userManagerFactory = userManagerFactory;
            _identityProvider = identityProvider;
            _log = log;
        }


        private readonly Func<UserManager<IdentityUser>> _userManagerFactory;
        private readonly IUserIdentityProvider _identityProvider;
        private readonly ILog _log;


        public override void Configure(IApplicationBuilder builder)
        {
            // Регистрация метода для создания менеджера управления пользователями
            // TODO Extension method deleted.
            //builder.CreatePerOwinContext(_userManagerFactory);

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
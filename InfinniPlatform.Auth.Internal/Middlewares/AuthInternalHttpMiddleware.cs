using System;

using InfinniPlatform.Auth.Internal.Identity;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;

using Owin;

namespace InfinniPlatform.Auth.Internal.Middlewares
{
    /// <summary>
    /// Промежуточный слой обработки HTTP запросов приложения для аутентификации пользователя на основе базы данных пользователей.
    /// </summary>
    internal class AuthInternalHttpMiddleware : HttpMiddleware
    {
        public AuthInternalHttpMiddleware(Func<UserManager<IdentityApplicationUser>> userManagerFactory,
                                          IUserIdentityProvider identityProvider,
                                          ILog log)
            : base(HttpMiddlewareType.InternalAuthentication)
        {
            _userManagerFactory = userManagerFactory;
            _identityProvider = identityProvider;
            _log = log;
        }


        private readonly Func<UserManager<IdentityApplicationUser>> _userManagerFactory;
        private readonly IUserIdentityProvider _identityProvider;
        private readonly ILog _log;


        public override void Configure(IAppBuilder builder)
        {
            // Регистрация метода для создания менеджера управления пользователями
            builder.CreatePerOwinContext(_userManagerFactory);

            // Прослойка для установки информации об идентификационных данных текущего пользователя
            builder.Use((owinContext, nextOwinMiddleware) =>
                        {
                            var requestUser = owinContext.Request.User;

                            _identityProvider.SetUserIdentity(requestUser);

                            _log.SetUserId(requestUser?.Identity);

                            return nextOwinMiddleware.Invoke();
                        });
        }
    }
}
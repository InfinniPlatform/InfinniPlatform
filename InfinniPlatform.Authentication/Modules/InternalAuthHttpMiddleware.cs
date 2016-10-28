using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Security;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;

using Microsoft.AspNet.Identity;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внутренней аутентификации.
    /// </summary>
    internal sealed class InternalAuthHttpMiddleware : HttpMiddleware
    {
        public InternalAuthHttpMiddleware(ILog log, IContainerResolver resolver) : base(HttpMiddlewareType.InternalAuthentication)
        {
            _log = log;
            _resolver = resolver;
        }


        private readonly ILog _log;
        private readonly IContainerResolver _resolver;


        public override void Configure(IAppBuilder builder)
        {
            // Регистрация метода для создания менеджера управления пользователями
            builder.CreatePerOwinContext(() => _resolver.Resolve<UserManager<IdentityApplicationUser>>());

            // Прослойка для установки информации об идентификационных данных текущего пользователя
            builder.Use((owinContext, nextOwinMiddleware) =>
                        {
                            var requestUser = owinContext.Request.User;

                            UserIdentityProvider.SetRequestUser(requestUser);

                            _log.SetUserId(requestUser?.Identity);

                            return nextOwinMiddleware.Invoke();
                        });
        }
    }
}
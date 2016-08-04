using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Security;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;

using Microsoft.AspNet.Identity;

using Owin;

namespace InfinniPlatform.Authentication.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов к подсистеме внутренней аутентификации.
    /// </summary>
    internal sealed class InternalAuthOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.InternalAuth;

        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            // Регистрация метода для создания менеджера управления пользователями
            builder.CreatePerOwinContext(() => context.ContainerResolver.Resolve<UserManager<IdentityApplicationUser>>());

            // Прослойка для установки информации об идентификационных данных текущего пользователя
            builder.Use((owinContext, nextOwinMiddleware) =>
                        {
                            UserIdentityProvider.SetRequestUser(owinContext.Request.User);
                            log.SetUserId(owinContext.Request.User?.Identity);
                            return nextOwinMiddleware.Invoke();
                        });
        }
    }
}
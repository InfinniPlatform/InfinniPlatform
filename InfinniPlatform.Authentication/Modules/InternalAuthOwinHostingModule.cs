using System.Threading;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Owin.Modules;

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

        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            // Регистрация метода для создания менеджера управления пользователями
            builder.CreatePerOwinContext(() => context.ContainerResolver.Resolve<UserManager<IdentityApplicationUser>>());

            // Прослойка для установки Thread.CurrentPrincipal
            builder.Use((owinContext, nextOwinMiddleware) =>
                        {
                            Thread.CurrentPrincipal = owinContext.Request.User;
                            return nextOwinMiddleware.Invoke();
                        });
        }
    }
}
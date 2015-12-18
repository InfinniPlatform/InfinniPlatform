using System.Collections.Generic;

using InfinniPlatform.Owin.Middleware;

using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    /// Модуль хостинга приложений на платформе
    /// </summary>
    internal sealed class ApplicationSdkOwinMiddleware : RoutingOwinMiddleware
    {
        public ApplicationSdkOwinMiddleware(OwinMiddleware next, IEnumerable<IHandlerRegistration> handlers)
            : base(next)
        {
            foreach (var handler in handlers)
            {
                RegisterHandler(handler);
            }
        }
    }
}
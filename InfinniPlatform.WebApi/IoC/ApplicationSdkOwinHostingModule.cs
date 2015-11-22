using InfinniPlatform.Owin.Modules;
using InfinniPlatform.WebApi.Middleware;

using Owin;

namespace InfinniPlatform.WebApi.IoC
{
    /// <summary>
    /// Модуль хостинга обработки прикладных запросов (на базе прямой обработки запросов).
    /// </summary>
    /// <remarks>
    /// Новый и неиспользуемый на данный момент набор сервисов.
    /// </remarks>
    internal sealed class ApplicationSdkOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.Use(context.OwinMiddlewareResolver.ResolveType<ApplicationHostingRoutingMiddleware>());
        }
    }
}
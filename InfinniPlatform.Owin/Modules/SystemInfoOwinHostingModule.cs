using InfinniPlatform.Owin.Middleware;

using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов информации о системе.
    /// </summary>
    internal sealed class SystemInfoOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.SystemInfo;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.Use(context.OwinMiddlewareResolver.ResolveType<SystemInfoOwinMiddleware>());
        }
    }
}
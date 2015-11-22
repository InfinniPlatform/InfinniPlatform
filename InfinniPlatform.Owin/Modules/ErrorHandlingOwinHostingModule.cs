using InfinniPlatform.Owin.Middleware;

using Owin;

namespace InfinniPlatform.Owin.Modules
{
    /// <summary>
    /// Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal sealed class ErrorHandlingOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.ErrorHandling;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.Use(context.OwinMiddlewareResolver.ResolveType<ErrorHandlingOwinMiddleware>());
        }
    }
}
using InfinniPlatform.Sdk.Hosting;

using Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal class ErrorHandlingOwinHostingMiddleware : OwinHostingMiddleware
    {
        public ErrorHandlingOwinHostingMiddleware(IOwinMiddlewareResolver middlewareResolver) : base(HostingMiddlewareType.ErrorHandling)
        {
            _middlewareResolver = middlewareResolver;
        }


        private readonly IOwinMiddlewareResolver _middlewareResolver;


        public override void Configure(IAppBuilder builder)
        {
            builder.Use(_middlewareResolver.ResolveType<ErrorHandlingOwinMiddleware>());
        }
    }
}
using InfinniPlatform.Http.Middlewares;

using Owin;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal class ErrorHandlingHttpMiddleware : HttpMiddleware
    {
        public ErrorHandlingHttpMiddleware(IOwinMiddlewareTypeResolver typeResolver) : base(HttpMiddlewareType.ErrorHandling)
        {
            _typeResolver = typeResolver;
        }


        private readonly IOwinMiddlewareTypeResolver _typeResolver;


        public override void Configure(IAppBuilder builder)
        {
            builder.Use(_typeResolver.ResolveType<ErrorHandlingOwinMiddleware>());
        }
    }
}
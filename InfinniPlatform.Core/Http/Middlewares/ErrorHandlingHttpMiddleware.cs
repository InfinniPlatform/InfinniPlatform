using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;


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


        public override void Configure(IApplicationBuilder appBuilder)
        {
            //TODO Find way to extend OWIN pipelines in ASP.NET Core.
            //appBuilder.UseOwin(_typeResolver.ResolveType<ErrorHandlingOwinMiddleware>());
        }
    }
}
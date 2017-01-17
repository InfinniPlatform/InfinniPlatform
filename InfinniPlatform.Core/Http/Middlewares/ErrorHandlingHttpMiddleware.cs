using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    ///     Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    internal class ErrorHandlingHttpMiddleware : HttpMiddleware
    {
        public ErrorHandlingHttpMiddleware() : base(HttpMiddlewareType.ErrorHandling)
        {
        }


        public override void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<ErrorHandlingOwinMiddleware>();
        }
    }
}
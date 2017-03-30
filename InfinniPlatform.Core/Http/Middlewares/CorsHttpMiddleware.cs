using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга обработчика запросов CORS (Cross-origin resource sharing).
    /// </summary>
    internal sealed class CorsHttpMiddleware : HttpMiddleware
    {
        public CorsHttpMiddleware() : base(HttpMiddlewareType.BeforeAuthentication)
        {
        }


        public override void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.UseCors("AllowAllOrigins");
        }
    }
}
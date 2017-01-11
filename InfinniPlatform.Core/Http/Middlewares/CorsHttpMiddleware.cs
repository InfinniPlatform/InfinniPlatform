using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

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
            // TODO: Добавить правила CORS проверки из конфигурации
            var corsOptions = new CorsOptions();
            corsOptions.AddPolicy("AllowAllOrigins",
                                  builder => { builder.AllowAnyOrigin(); });

            appBuilder.UseCors("AllowAllOrigins");
        }
    }
}
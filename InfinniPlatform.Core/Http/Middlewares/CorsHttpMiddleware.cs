using InfinniPlatform.Http.Middlewares;

using Microsoft.Owin.Cors;

using Owin;

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


        public override void Configure(IAppBuilder builder)
        {
            // TODO: Добавить правила CORS проверки из конфигурации

            builder.UseCors(CorsOptions.AllowAll);
        }
    }
}
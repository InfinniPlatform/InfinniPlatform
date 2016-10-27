using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;

using Microsoft.Owin.Cors;

using Owin;

namespace InfinniPlatform.Cors.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов CORS (Cross-origin resource sharing).
    /// </summary>
    internal sealed class CorsOwinHostingMiddleware : OwinHostingMiddleware
    {
        public CorsOwinHostingMiddleware() : base(HostingMiddlewareType.BeforeAuthentication)
        {
        }


        public override void Configure(IAppBuilder builder)
        {
            // TODO: Добавить правила CORS проверки из конфигурации

            builder.UseCors(CorsOptions.AllowAll);
        }
    }
}
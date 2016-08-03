using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;

using Microsoft.Owin.Cors;

using Owin;

namespace InfinniPlatform.Cors.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов CORS (Cross-origin resource sharing).
    /// </summary>
    internal sealed class CorsOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Cors;


        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            // TODO: Добавить правила CORS проверки из конфигурации

            builder.UseCors(CorsOptions.AllowAll);
        }
    }
}
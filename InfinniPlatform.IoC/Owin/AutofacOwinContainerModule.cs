using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC.Owin
{
    internal class AutofacOwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Промежуточный слой для создания зависимостей на время обработки запроса
            builder.RegisterType<AutofacOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            // Провайдер разрешения типа обработчика запросов OWIN
            builder.RegisterType<AutofacOwinMiddlewareResolver>()
                   .As<IOwinMiddlewareResolver>()
                   .SingleInstance();
        }
    }
}
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.IoC.Http
{
    internal class AutofacHttpContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Промежуточный слой для создания зависимостей на время обработки запроса
            builder.RegisterType<AutofacHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Провайдер разрешения типа обработчика запросов OWIN
            builder.RegisterType<AutofacOwinMiddlewareResolver>()
                   .As<IOwinMiddlewareTypeResolver>()
                   .SingleInstance();
        }
    }
}
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Factories;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.WebApi.Factories;

using Owin;

namespace InfinniPlatform.WebApi.Modules
{
    /// <summary>
    /// Модуль хостинга обработки прикладных запросов (на базе ASP.NET WebApi).
    /// </summary>
    /// <remarks>
    /// Старый и используемый на данный момент набор сервисов.
    /// </remarks>
    internal sealed class ApplicationWebApiOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            var serviceRegistrationContainerFactory = context.ContainerResolver.Resolve<IServiceRegistrationContainerFactory>();
            var constructHostServer = InfinniPlatformHostServer.ConstructHostServer(serviceRegistrationContainerFactory);

            // Устанавливаем локальный роутинг для прохождения запросов внутри серверных потоков обработки запросов
            RestQueryApi.SetRoutingType(RoutingType.Local);
            RequestLocal.ApiControllerFactory = constructHostServer.GetApiControllerFactory();

            constructHostServer.Build(context);

            builder.UseWebApi(constructHostServer.HttpConfiguration);

            constructHostServer.OnStartHost(context);
        }
    }
}
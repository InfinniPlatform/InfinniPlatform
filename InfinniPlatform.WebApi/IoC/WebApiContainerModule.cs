using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.WebApi.Controllers;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.Middleware;
using InfinniPlatform.WebApi.Modules;
using InfinniPlatform.WebApi.WebApi;

namespace InfinniPlatform.WebApi.IoC
{
    internal sealed class WebApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ModuleComposer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ApiControllerFactory>()
                   .As<IApiControllerFactory>()
                   .SingleInstance();

            builder.RegisterType<ApplicationHostServer>()
                   .AsSelf()
                   .SingleInstance();

            // Модули обработки прикладных запросов

            builder.RegisterType<ApplicationWebApiOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ApplicationSdkOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ApplicationSdkOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<HttpResultHandlerFactory>()
                   .As<IHttpResultHandlerFactory>()
                   .SingleInstance();

            // Универсальные контроллеры

            builder.RegisterType<StandardApiController>()
                   .AsSelf();

            builder.RegisterType<UploadController>()
                   .AsSelf();

            builder.RegisterType<UrlEncodedDataController>()
                   .AsSelf();
        }
    }
}
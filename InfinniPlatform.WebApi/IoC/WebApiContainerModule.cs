using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.WebApi.Middleware;
using InfinniPlatform.WebApi.Middleware.StandardHandlers;
using InfinniPlatform.WebApi.Middleware.UserAuthHandlers;
using InfinniPlatform.WebApi.Modules;

namespace InfinniPlatform.WebApi.IoC
{
    internal sealed class WebApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Модули обработки прикладных запросов

            builder.RegisterType<ApplicationSdkOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ApplicationSdkOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            // Обработчики для ApplicationSdkOwinMiddleware

            builder.RegisterType<GetDocumentByIdHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetDocumentCountHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SetDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SetDocumentsHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<DeleteDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<AddUserClaimHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetUserClaimHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
        }
    }
}
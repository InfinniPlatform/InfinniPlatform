using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.WebApi.Controllers;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.Middleware;
using InfinniPlatform.WebApi.Middleware.SessionHandlers;
using InfinniPlatform.WebApi.Middleware.StandardHandlers;
using InfinniPlatform.WebApi.Middleware.UserAuthHandlers;
using InfinniPlatform.WebApi.Modules;
using InfinniPlatform.WebApi.ProvidersLocal;
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

            // Обработчики для ApplicationSdkOwinMiddleware

            builder.RegisterType<GetDocumentByIdHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetDocumentCountHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SetDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SetDocumentsHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<DeleteDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            
            builder.RegisterType<FileDownloadHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<FileUploadHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();

            builder.RegisterType<CustomServiceRegistrationHandler>().As<IHandlerRegistration>().SingleInstance();

            builder.RegisterType<CreateSessionHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetSessionByIdHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SessionCommitHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<SessionRemoveHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<FileAttachHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<FileDetachHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<AttachDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<DetachDocumentHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();

            builder.RegisterType<AddUserClaimHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();
            builder.RegisterType<GetUserClaimHandlerRegistration>().As<IHandlerRegistration>().SingleInstance();

            // Fury

            builder.RegisterType<ApiControllerLocal>()
                   .As<IRequestLocal>()
                   .SingleInstance();
        }
    }
}
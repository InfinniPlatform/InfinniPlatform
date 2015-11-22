using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.WebApi.Factories;
using InfinniPlatform.WebApi.Modules;

namespace InfinniPlatform.WebApi.IoC
{
    internal sealed class WebApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ApplicationWebApiOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ApplicationSdkOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<HttpResultHandlerFactory>()
                   .As<IHttpResultHandlerFactory>()
                   .SingleInstance();
        }
    }
}
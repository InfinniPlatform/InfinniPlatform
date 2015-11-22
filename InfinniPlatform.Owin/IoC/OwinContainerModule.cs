using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Owin.IoC
{
    internal sealed class OwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ErrorHandlingOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<SystemInfoOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();
        }
    }
}
using InfinniPlatform.Cors.Modules;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Cors.IoC
{
    internal sealed class CorsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<CorsOwinHostingModule>()
                .As<IOwinHostingModule>()
                .SingleInstance();
        }
    }
}
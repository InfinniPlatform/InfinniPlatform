using InfinniPlatform.Cors.Modules;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Cors.IoC
{
    internal sealed class CorsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<CorsOwinHostingMiddleware>()
                .As<IHostingMiddleware>()
                .SingleInstance();
        }
    }
}
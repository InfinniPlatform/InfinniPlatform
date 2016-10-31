using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.Diagnostics
{
    internal class DiagnosticsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SystemInfoHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();
        }
    }
}
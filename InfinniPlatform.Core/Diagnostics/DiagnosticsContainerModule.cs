using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.IoC;

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
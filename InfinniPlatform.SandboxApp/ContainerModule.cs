using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.SandboxApp.Models;

namespace InfinniPlatform.SandboxApp
{
    public class ContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<LogContextLayer>()
                   .As<IDefaultAppLayer>()
                   .SingleInstance();

            builder.RegisterDocumentHttpService<Entity>();
        }
    }
}
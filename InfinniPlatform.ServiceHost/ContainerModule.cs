using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;

namespace InfinniPlatform.ServiceHost
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
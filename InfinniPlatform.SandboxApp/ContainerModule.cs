using InfinniPlatform.DocumentStorage;
using InfinniPlatform.IoC;
using InfinniPlatform.SandboxApp.Models;

namespace InfinniPlatform.SandboxApp
{
    public class ContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterDocumentHttpService<Entity>();
        }
    }
}
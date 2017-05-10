using System.Reflection;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.ServiceHost
{
    public class ContainerBuilder : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(typeof(ContainerBuilder).GetTypeInfo().Assembly);
        }
    }
}
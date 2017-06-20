using System.Reflection;
using InfinniPlatform.Aspects;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;
using InfinniPlatform.ServiceHost.Interception;

namespace InfinniPlatform.ServiceHost
{
    public class ContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Register dependencies
            builder.RegisterHttpServices(GetType().GetTypeInfo().Assembly);

            builder.RegisterType<AsyncClass>()
                   .As<IAsyncInterface>()
                   .SingleInstance();

            builder.RegisterType<TaskClass>()
                   .As<ITaskInterface>()
                   .SingleInstance();

            builder.RegisterType<SyncClass>()
                   .As<ISyncInterface>()
                   .SingleInstance();
        }
    }
}
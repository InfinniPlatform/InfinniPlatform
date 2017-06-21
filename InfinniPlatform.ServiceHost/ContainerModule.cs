using System.Reflection;
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

            builder.RegisterType<TestClasses.AsyncClass>()
                   .As<TestInterfaces.IAsyncInterface>()
                   .SingleInstance();

            builder.RegisterType<TestClasses.TaskClass>()
                   .As<TestInterfaces.ITaskInterface>()
                   .SingleInstance();

            builder.RegisterType<TestClasses.SyncClass>()
                   .As<TestInterfaces.ISyncInterface>()
                   .SingleInstance();
        }
    }
}
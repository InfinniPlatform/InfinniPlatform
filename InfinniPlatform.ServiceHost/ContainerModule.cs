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

            builder.RegisterType<TestClass>()
                   .As<ITestInterface>()
                   .SingleInstance();

            builder.RegisterType<InterceptedTestClass>()
                   .As<IInterceptedTestInterface>()
                   .SingleInstance();
        }
    }
}
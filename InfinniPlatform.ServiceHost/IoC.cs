using System.Reflection;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.ServiceHost
{
    public class IoC: IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<MyClass>()
                   .As<IInterface>()
                   .InstancePerLifetimeScope();

            builder.RegisterHttpServices(GetType().GetTypeInfo().Assembly);
        }
    }
}
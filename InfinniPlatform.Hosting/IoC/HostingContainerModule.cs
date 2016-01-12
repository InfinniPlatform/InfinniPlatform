using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Hosting.IoC
{
    internal sealed class HostingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ServiceTemplateConfiguration>()
                   .As<IServiceTemplateConfiguration>()
                   .SingleInstance();

            builder.RegisterType<ServiceRegistrationContainerFactory>()
                   .As<IServiceRegistrationContainerFactory>()
                   .SingleInstance();
        }
    }
}
using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ServiceRegistration
{
    public sealed class ServiceRegistrationContainerFactory : IServiceRegistrationContainerFactory
    {
        public ServiceRegistrationContainerFactory(IServiceTemplateConfiguration serviceTemplateConfiguration)
        {
            ServiceTemplateConfiguration = serviceTemplateConfiguration;
        }

        public IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }

        public IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId)
        {
            return new ServiceRegistrationContainer(ServiceTemplateConfiguration, metadataConfigurationId);
        }
    }
}
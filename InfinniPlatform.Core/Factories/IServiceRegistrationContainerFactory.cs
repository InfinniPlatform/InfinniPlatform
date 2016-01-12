using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.Core.Factories
{
    public interface IServiceRegistrationContainerFactory
    {
        IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }
        IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId);
    }
}
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Core.Factories
{
    public interface IServiceRegistrationContainerFactory
    {
        IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }
        IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId);
    }
}
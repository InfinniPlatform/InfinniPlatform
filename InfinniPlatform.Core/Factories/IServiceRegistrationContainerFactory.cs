using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Factories
{
    public interface IServiceRegistrationContainerFactory
    {
        IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }
        IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId);
    }
}
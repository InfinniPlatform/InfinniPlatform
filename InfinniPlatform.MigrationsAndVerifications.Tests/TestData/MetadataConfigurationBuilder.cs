using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Metadata;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Metadata.Implementation.MetadataConfiguration;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.MigrationsAndVerifications.Tests.TestData
{
    public static class MetadataConfigurationBuilder
    {
        public static IMetadataConfigurationProvider BuildMetadataConfigurationProvider()
        {
            var templateConfig = new ServiceTemplateConfiguration().CreateDefaultServiceConfiguration();
            var metadataConfigurationProvider = new MetadataConfigurationProvider(new ServiceRegistrationContainerFactory(templateConfig), templateConfig);
            return metadataConfigurationProvider;
        }

    }
}

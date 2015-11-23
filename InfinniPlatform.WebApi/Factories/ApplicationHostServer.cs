using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.WebApi.Factories
{
    /// <summary>
    /// Сервис установки и удаления прикладных сервисов.
    /// </summary>
    public class ApplicationHostServer
    {
        private ApplicationHostServer(ModuleComposer moduleComposer, IApiControllerFactory apiControllerFactory, IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _moduleComposer = moduleComposer;
            _apiControllerFactory = apiControllerFactory;
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        private readonly ModuleComposer _moduleComposer;
        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


        public void RegisterServices()
        {
            _moduleComposer.RegisterTemplates();
            _moduleComposer.RegisterModules();
        }


        public IMetadataConfiguration CreateConfiguration(string configId, bool isEmbeddedConfiguration, string version)
        {
            _apiControllerFactory.RegisterVersion(configId, version);

            return _metadataConfigurationProvider.AddConfiguration(configId, isEmbeddedConfiguration);
        }

        public void RemoveConfiguration(string configId)
        {
            _apiControllerFactory.UnregisterVersion(configId, null);

            _metadataConfigurationProvider.RemoveConfiguration(configId);
        }


        public void InstallServices(string version, IServiceRegistrationContainer serviceRegistrationContainer)
        {
            var configId = serviceRegistrationContainer.MetadataConfigurationId;

            foreach (var serviceType in serviceRegistrationContainer.Registrations)
            {
                var restVerbsRegistrator = _apiControllerFactory.CreateTemplate(version, configId, serviceType.MetadataName);
                restVerbsRegistrator.AddVerb(serviceType.QueryHandler);
            }
        }

        public void UninstallServices(string configId)
        {
            _apiControllerFactory.RemoveTemplates(null, configId);
        }
    }
}
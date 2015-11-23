using System.Collections.Generic;

using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Modules;

namespace InfinniPlatform.WebApi.Factories
{
    public class ModuleComposer
    {
        public ModuleComposer(IApiControllerFactory apiControllerFactory, IEnumerable<IModuleInstaller> moduleInstallers, IEnumerable<ITemplateInstaller> templateInstallers)
        {
            _apiControllerFactory = apiControllerFactory;
            _moduleInstallers = moduleInstallers;
            _templateInstallers = templateInstallers;
        }


        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IEnumerable<IModuleInstaller> _moduleInstallers;
        private readonly IEnumerable<ITemplateInstaller> _templateInstallers;


        public void RegisterModules()
        {
            foreach (var moduleInstaller in _moduleInstallers)
            {
                var module = moduleInstaller.InstallModule();

                var serviceRegistrationContainer = module.ServiceRegistrationContainer;
                var metadataConfigurationId = serviceRegistrationContainer.MetadataConfigurationId;

                foreach (var serviceType in serviceRegistrationContainer.Registrations)
                {
                    var restVerbsRegistrator = _apiControllerFactory.CreateTemplate(metadataConfigurationId, serviceType.MetadataName);

                    restVerbsRegistrator.AddVerb(serviceType.QueryHandler);
                }
            }
        }

        public void RegisterTemplates()
        {
            foreach (var templateInstaller in _templateInstallers)
            {
                templateInstaller.RegisterTemplates();
            }
        }
    }
}
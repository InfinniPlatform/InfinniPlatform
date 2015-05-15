using System.Collections.Generic;
using System.IO;
using InfinniPlatform.Actions;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.File;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.Modules;
using InfinniPlatform.SystemConfig.RoutingFactory;
using InfinniPlatform.WebApi.WebApi;

namespace InfinniPlatform.SystemConfig.LocalHost
{
    public sealed class LocalHostServer : ILocalHostServer
    {
        private readonly string _pathToConfigurationsFolder = Directory.GetCurrentDirectory();

        private readonly string _pathToAssembliesFolder = Directory.GetCurrentDirectory();

        private readonly IList<string> _configurationsToStart = new List<string>();
		private readonly List<KeyValuePair<string,string>> _assembliesToLoad = new List<KeyValuePair<string,string>>(); 

        private readonly ApiControllerFactory _apiControllerFactory;

        private readonly ApiControllerFactoryBuilder _apiControllerFactoryBuilder = new ApiControllerFactoryBuilder();
        private readonly ServiceTemplateConfiguration _serviceTemplateConfiguration;

	    private readonly IIndexFactory _indexFactory = new ElasticFactory(new RoutingFactoryBase());

		private readonly ILogFactory _logFactory = new Log4NetLogFactory();

        public LocalHostServer()
        {
	        var pathToConfigurationsFolder = AppSettings.GetValue("ConfigurationPath");
	        var pathToAssembliesFolder = AppSettings.GetValue("AppliedAssemblies");

            if (!string.IsNullOrEmpty(pathToConfigurationsFolder))
            {
                _pathToConfigurationsFolder = pathToConfigurationsFolder;
            }

            if (!string.IsNullOrEmpty(pathToAssembliesFolder))
            {
                _pathToAssembliesFolder = pathToAssembliesFolder;
            }

            _serviceTemplateConfiguration = new ServiceTemplateConfiguration();
            _apiControllerFactoryBuilder.RegisterSingleInstance(_serviceTemplateConfiguration);
            _apiControllerFactoryBuilder.RegisterSingleInstance(new ServiceRegistrationContainerFactory(_serviceTemplateConfiguration));           


            _apiControllerFactory = _apiControllerFactoryBuilder.BuildApiFactory();
            
        }

        public ApiControllerFactory ApiControllerFactory
        {
            get { return _apiControllerFactory; }
        }

        public void RegisterStartConfiguration(string configurationId)
        {
            _configurationsToStart.Add(configurationId);
        }


		public void RegisterAssembly(string configId, string assemblyName)
		{
			_assembliesToLoad.Add(new KeyValuePair<string, string>(configId,assemblyName));
		}

        public void Start()
        {
            RestQueryApi.SetRoutingType(RoutingType.Local);
			RestQueryApi.SetProfilingType(ProfilingType.ProfileWatch);
	        RestQueryApi.Log = _logFactory.CreateLog();
            RequestLocal.ApiControllerFactory = ApiControllerFactory;

            InstallSystemServices();

            UpdateSystemAssemblies();

            var manager = new JsonFileConfigManager(_pathToConfigurationsFolder);

            var configReader = new JsonConfigReaderFile(manager);

            var installer = new JsonConfigurationInstaller(configReader);


            IGlobalContext context = (IGlobalContext)_apiControllerFactoryBuilder.ResolveInstance(typeof(IGlobalContext));

            context.ManagerIdentifiers = manager;
            context.ConfigurationReader = configReader;

            manager.ReadConfigurations();

			var metadataConfigurationProvider = (IMetadataConfigurationProvider)_apiControllerFactoryBuilder.ResolveInstance(typeof(IMetadataConfigurationProvider));

            foreach (var configId in _configurationsToStart)
            {
                
                var actionConfig = (IScriptConfiguration) _apiControllerFactoryBuilder.ResolveInstance(typeof(IScriptConfiguration));
                IMetadataConfiguration metadataConfig = metadataConfigurationProvider.AddConfiguration(configId, actionConfig, false);               

                installer.InstallConfiguration(metadataConfig);
                metadataConfig.ScriptConfiguration.InitActionUnitStorage();

                InstallTemplates(metadataConfig);
            }

			CreateStore(metadataConfigurationProvider);

			UpdateAppliedAssemblies();

	        foreach (var configId in _configurationsToStart)
	        {
				UpdateApi.UpdateStore(configId);
	        }
        }

	    private void CreateStore(IMetadataConfigurationProvider metadataConfigurationProvider)
	    {
			CreateSystemStore(metadataConfigurationProvider.GetMetadataConfiguration("Update"));
			CreateSystemStore(metadataConfigurationProvider.GetMetadataConfiguration("RestfulApi"));
			CreateSystemStore(metadataConfigurationProvider.GetMetadataConfiguration("SystemConfig"));

		    foreach (var configId in _configurationsToStart)
		    {
			    CreateSystemStore(metadataConfigurationProvider.GetMetadataConfiguration(configId));
		    }

	    }

	    public void Stop()
		{
			RestQueryApi.SetRoutingType(RoutingType.Remote);
			RestQueryApi.SetProfilingType(ProfilingType.None);
		    RestQueryApi.Log = null;
			RequestLocal.ApiControllerFactory = null;
		}


        private void UpdateSystemAssemblies()
        {
            var packageBuilder = new PackageBuilder();

            var package1 = packageBuilder.BuildPackage("Update", "version_update",
                                        Path.Combine(_pathToAssembliesFolder, "InfinniPlatform.Update.dll"));
            var package2 = packageBuilder.BuildPackage("RestfulApi", "version_restfulapi",
                                        Path.Combine(_pathToAssembliesFolder, "InfinniPlatform.RestfulApi.exe"));
            var package3 = packageBuilder.BuildPackage("SystemConfig", "version_systemconfig",
                                        Path.Combine(_pathToAssembliesFolder, "InfinniPlatform.SystemConfig.dll"));


            UpdateApi.InstallPackages(new[] { package1, package2, package3 });
        }

		private void UpdateAppliedAssemblies()
		{
			var packageBuilder = new PackageBuilder();

			var packagesList = new List<dynamic>();

			foreach (var assembly in _assembliesToLoad)
			{
				var package = packageBuilder.BuildPackage(assembly.Key, "test_version", Path.Combine(_pathToAssembliesFolder, assembly.Value));
				packagesList.Add(package);
			}
			UpdateApi.InstallPackages(packagesList);

			
		}

        private void InstallTemplates(IMetadataConfiguration metadataConfig)
        {
            foreach (var serviceType in metadataConfig.ServiceRegistrationContainer.Registrations)
            {
                _apiControllerFactory.CreateTemplate(metadataConfig.ServiceRegistrationContainer.MetadataConfigurationId,
                                                     serviceType.MetadataName).AddVerb(serviceType.QueryHandler);
            }
        }

        private void InstallSystemServices()
        {
            var modules = ModuleExtension.LoadModulesAssemblies(
                "InfinniPlatform.SystemConfig,InfinniPlatform.Metadata,InfinniPlatform.Update,InfinniPlatform.RestfulApi");

            var moduleComposer = _apiControllerFactoryBuilder.BuildModuleComposer(modules);
            moduleComposer.RegisterTemplates();
            foreach (var registeredType in _serviceTemplateConfiguration.GetRegisteredTypes())
            {
                _apiControllerFactoryBuilder.UpdateRegisterTypePerDependency(registeredType);
            }
            _apiControllerFactoryBuilder.Update();

            var systemConfigurations = moduleComposer.RegisterModules();
            foreach (var systemConfiguration in systemConfigurations)
            {
                InstallTemplates((IMetadataConfiguration)systemConfiguration);
            }

        }

		private void CreateSystemStore(IMetadataConfiguration metadataConfiguration)
		{
			
			if (metadataConfiguration != null)
			{
				var containers = metadataConfiguration.Containers;
				foreach (var containerId in containers)
				{
					var versionBuilder = _indexFactory.BuildVersionBuilder(
						metadataConfiguration.ConfigurationId,
						metadataConfiguration.GetMetadataIndexType(containerId),
						metadataConfiguration.GetSearchAbilityType(containerId));

					// Для системных конфигураций использована упрощенная схема
					// создания хранилищ - без учета схем данных документов.
					// На момент написания кода ни для одного документа системной
					// конфигурации мапинг задан не был. Если в дальнейшем
					// по каким-то причинам будет необходимо учитывать схему
					// данных системных документов, необходимо будет использовать механизм
					// UpdateStoreMigration

					if (!versionBuilder.VersionExists())
					{
						versionBuilder.CreateVersion();
					}
				}
			}
		}


    }
}

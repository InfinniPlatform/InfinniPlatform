using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using InfinniPlatform.Api.Context;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.WebApi.WebApi
{
    public class ApiControllerFactoryBuilder
    {
        private ContainerBuilder _containerBuilder;

        private ContainerBuilder _updateContainerBuilder;

        private IContainer _container;


        public ApiControllerFactoryBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            _updateContainerBuilder = new ContainerBuilder();
            
        }

        private IContainer Container
        {
           get { return _container ?? (_container = ContainerBuilder.Build()); }
        }

        private ContainerBuilder ContainerBuilder
        {
            get { return _containerBuilder; }
        }

        public ApiControllerFactory BuildApiFactory()
        {
            var configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            InfinniPlatform.Logging.Logger.Log.Info($"Register Autofac configuration {configFile}");
            _containerBuilder.RegisterModule(new ConfigurationSettingsReader("autofac", configFile));

            return new ApiControllerFactory(() => Container);
        }

        public ModuleComposer BuildModuleComposer(IEnumerable<Assembly> modules)
        {
            return new ModuleComposer(modules, ContainerBuilder, () => Container);
        }

        public void RegisterSingleInstance(object instance)
        {
            ContainerBuilder.RegisterInstance(instance).AsSelf().AsImplementedInterfaces().SingleInstance();
            
        }

        public void UpdateRegisterTypePerDependency(Type registeredType)
        {
            _updateContainerBuilder.RegisterType(registeredType).AsSelf().InstancePerDependency();
        }


        public void Update()
        {
            _updateContainerBuilder.Update(Container);
        }

        public object ResolveInstance(Type type)
        {
            return Container.Resolve(type);
        }
    }
}

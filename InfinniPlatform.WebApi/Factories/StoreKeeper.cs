using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using InfinniPlatform.Factories;
using InfinniPlatform.Modules;
using InfinniPlatform.WebApi.WebApi;

namespace InfinniPlatform.WebApi.Factories
{
    public sealed class StoreKeeper
    {
        private readonly IEnumerable<Assembly> _assemblies;
        private readonly ContainerBuilder _containerBuilder;

        public StoreKeeper(HostServer server, IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _containerBuilder = server.ContainerBuilder;
        }

        public void CreateStore(HostServer server)
        {
            var storeInstallerTypes = GetConfigurationStoreBuilders();
            server.AfterCreateContainer(() =>
                {
                    var container = server.Container();
                    
                    foreach (var storeInstallerType in storeInstallerTypes)
                    {
                        var storeInstaller = (IConfigurationStoreInstaller) container.Resolve(storeInstallerType);
                        storeInstaller.InstallStore();
                    }
                });

            server.ActionUpdateContainer((containerBuilder) =>
                {
                    foreach (var storeInstallerType in storeInstallerTypes)
                    {
                        containerBuilder.RegisterType(storeInstallerType);
                    }
                    
                });
        }

        private IEnumerable<Type> GetConfigurationStoreBuilders()
        {
            var builders = new List<Type>();
            _assemblies.ToList().ForEach(
                a => a.GetTypes().Where(t => !t.IsAbstract).ToList().ForEach(f =>
                {
                    if (f.GetInterfaces().Contains(typeof(IConfigurationStoreInstaller)))
                    {
                        builders.Add(f);
                        _containerBuilder.RegisterType(f).AsImplementedInterfaces().AsSelf().SingleInstance();
                    }
                }));
            return builders;
        }
    }
}

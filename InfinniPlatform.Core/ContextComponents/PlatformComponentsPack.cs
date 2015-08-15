using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Api.Versioning;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///   Пакет компонентов платформы
    /// </summary>
    public sealed class PlatformComponentsPack : IPlatformComponentsPack
    {
        private static readonly ISystemComponent SystemComponent = new SystemComponent();
        private readonly IList<ContextRegistration> _components = new List<ContextRegistration>();

        public PlatformComponentsPack(IDependencyContainerComponent dependencyContainerComponent)
        {
            var eventStorage =
                 dependencyContainerComponent.ResolveDependency<IEventStorageFactory>().CreateEventStorage();
            var blobStorage = dependencyContainerComponent.ResolveDependency<IBlobStorageFactory>().CreateBlobStorage();
            var printViewBuilder =
                dependencyContainerComponent.ResolveDependency<IPrintViewBuilderFactory>().CreatePrintViewBuilder();
            var logger = dependencyContainerComponent.ResolveDependency<ILogFactory>().CreateLog();
            var metadataConfigurationProvider =
                dependencyContainerComponent.ResolveDependency<IMetadataConfigurationProvider>();
            var versionStrategy = dependencyContainerComponent.ResolveDependency<IVersionStrategy>();

            var sharedCacheComponent = dependencyContainerComponent.ResolveDependency<ISharedCacheComponent>();

            _components.Add(new ContextRegistration(typeof(ISharedCacheComponent), () => sharedCacheComponent));

            _components.Add(new ContextRegistration(typeof(IVersionStrategy), () => versionStrategy));

            _components.Add(new ContextRegistration(typeof(IBlobStorageComponent),
                () => new BlobStorageComponent(blobStorage)));
            _components.Add(new ContextRegistration(typeof(IEventStorageComponent),
                () => new EventStorageComponent(eventStorage)));
            _components.Add(new ContextRegistration(typeof(IIndexComponent),
                () => new IndexComponent(dependencyContainerComponent.ResolveDependency<IIndexFactory>())));
            _components.Add(new ContextRegistration(typeof(ILogComponent), () => new LogComponent(logger)));
            _components.Add(new ContextRegistration(typeof(IMetadataComponent),
                () => new MetadataComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ICrossConfigSearchComponent),
                () => new CrossConfigSearchComponent(dependencyContainerComponent.ResolveDependency<ICrossConfigSearcher>())));
            _components.Add(new ContextRegistration(typeof(IPrintViewComponent),
                () => new PrintViewComponent(printViewBuilder)));
            _components.Add(new ContextRegistration(typeof(IProfilerComponent),
                () => new ProfilerComponent(logger)));
            _components.Add(new ContextRegistration(typeof(IRegistryComponent),
                () => new RegistryComponent()));
            _components.Add(new ContextRegistration(typeof(IScriptRunnerComponent),
                () => new ScriptRunnerComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ISecurityComponent),
                dependencyContainerComponent.ResolveDependency<ISecurityComponent>));
            _components.Add(new ContextRegistration(typeof(ITransactionComponent),
                () =>
                    new TransactionComponent(dependencyContainerComponent.ResolveDependency<ITransactionManager>())));
            _components.Add(new ContextRegistration(typeof(IWebClientNotificationComponent),
                () =>
                    new WebClientNotificationComponent(
                        dependencyContainerComponent.ResolveDependency<IWebClientNotificationServiceFactory>())));
            _components.Add(new ContextRegistration(typeof(IConfigurationMediatorComponent),
                () => new ConfigurationMediatorComponent(
                    dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>())));

            _components.Add(new ContextRegistration(typeof(IDependencyContainerComponent),
                () => dependencyContainerComponent));
            _components.Add(new ContextRegistration(typeof(ISystemComponent), () => SystemComponent));
            
            _components.Add(new ContextRegistration(typeof(IMetadataConfigurationProvider),
                () => metadataConfigurationProvider));

        }

        public T GetComponent<T>() where T : class
        {
            return
                _components.Where(c => c.IsTypeOf(typeof(T))).Select(c => c.GetInstance()).FirstOrDefault() as T;
        }
    }
}

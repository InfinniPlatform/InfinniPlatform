using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.Sdk.Contracts;

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

            _components.Add(new ContextRegistration(typeof(IBlobStorageComponent),
                version => new BlobStorageComponent(blobStorage)));
            _components.Add(new ContextRegistration(typeof(IEventStorageComponent),
                version => new EventStorageComponent(eventStorage)));
            _components.Add(new ContextRegistration(typeof(IIndexComponent),
                version => new IndexComponent(dependencyContainerComponent.ResolveDependency<IIndexFactory>())));
            _components.Add(new ContextRegistration(typeof(ILogComponent), version => new LogComponent(logger)));
            _components.Add(new ContextRegistration(typeof(IMetadataComponent),
                version => new MetadataComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ICrossConfigSearchComponent),
                version =>
                    new CrossConfigSearchComponent(
                        dependencyContainerComponent.ResolveDependency<ICrossConfigSearcher>())));
            _components.Add(new ContextRegistration(typeof(IPrintViewComponent),
                version => new PrintViewComponent(printViewBuilder)));
            _components.Add(new ContextRegistration(typeof(IProfilerComponent),
                version => new ProfilerComponent(logger)));
            _components.Add(new ContextRegistration(typeof(IRegistryComponent),
                version => new RegistryComponent(version)));
            _components.Add(new ContextRegistration(typeof(IScriptRunnerComponent),
                version => new ScriptRunnerComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ISecurityComponent),
                version => new CachedSecurityComponent()));
            _components.Add(new ContextRegistration(typeof(ITransactionComponent),
                version =>
                    new TransactionComponent(dependencyContainerComponent.ResolveDependency<ITransactionManager>())));
            _components.Add(new ContextRegistration(typeof(IWebClientNotificationComponent),
                version =>
                    new WebClientNotificationComponent(
                        dependencyContainerComponent.ResolveDependency<IWebClientNotificationServiceFactory>())));
            _components.Add(new ContextRegistration(typeof(IConfigurationMediatorComponent),
                version => new ConfigurationMediatorComponent(
                    dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>())));

            _components.Add(new ContextRegistration(typeof(IDependencyContainerComponent),
                version => dependencyContainerComponent));
            _components.Add(new ContextRegistration(typeof(ISystemComponent), version => SystemComponent));
            
            _components.Add(new ContextRegistration(typeof(IMetadataConfigurationProvider),
                version => metadataConfigurationProvider));

        }

        public T GetComponent<T>(string version) where T : class
        {
            return
                _components.Where(c => c.IsTypeOf(typeof(T))).Select(c => c.GetInstance(version)).FirstOrDefault() as T;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.Factories
{
	public class GlobalContext : IGlobalContext, IComponentContainer
	{	
		private static readonly ISystemComponent SystemComponent = new SystemComponent();

		public GlobalContext(IDependencyContainerComponent dependencyContainerComponent)
		{
			var eventStorage = dependencyContainerComponent.ResolveDependency<IEventStorageFactory>().CreateEventStorage();
			var blobStorage = dependencyContainerComponent.ResolveDependency<IBlobStorageFactory>().CreateBlobStorage();
			var printViewBuilder = dependencyContainerComponent.ResolveDependency<IPrintViewBuilderFactory>().CreatePrintViewBuilder();
			var logger = dependencyContainerComponent.ResolveDependency<ILogFactory>().CreateLog();
			var metadataConfigurationProvider = dependencyContainerComponent.ResolveDependency<IMetadataConfigurationProvider>();

			_components.Add(new BlobStorageComponent(blobStorage));
			_components.Add(new EventStorageComponent(eventStorage));
			_components.Add(new IndexComponent(dependencyContainerComponent.ResolveDependency<IIndexFactory>()));
			_components.Add(new LogComponent(logger));
			_components.Add(new MetadataComponent(metadataConfigurationProvider));
			_components.Add(new CrossConfigSearchComponent(dependencyContainerComponent.ResolveDependency<ICrossConfigSearcher>()));
			_components.Add(new PrintViewComponent(printViewBuilder));
			_components.Add(new ProfilerComponent(logger));
			_components.Add(new RegistryComponent());
			_components.Add(new ScriptRunnerComponent(metadataConfigurationProvider));
			_components.Add(new CachedSecurityComponent());
			_components.Add(new TransactionComponent(dependencyContainerComponent.ResolveDependency<ITransactionManager>()));
			_components.Add(new WebClientNotificationComponent(dependencyContainerComponent.ResolveDependency<IWebClientNotificationServiceFactory>()));
			_components.Add(new ConfigurationMediatorComponent(
				dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>()));

			_components.Add(dependencyContainerComponent);
			_components.Add(SystemComponent);

			_components.Add(new DocumentApi());
			_components.Add(new DocumentApiUnsecured());
			_components.Add(new PrintViewApi());
			_components.Add(new RegisterApi());
			_components.Add(new ReportApi());
			_components.Add(new UploadApi());
			_components.Add(new MetadataApi());
			_components.Add(new AuthApi());
			_components.Add(new SignInApi());
			_components.Add(new PasswordVerifierComponent(this));
			_components.Add(new InprocessDocumentComponent(new ConfigurationMediatorComponent(
																dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>()                                                                
																),
														   new CachedSecurityComponent(),
                                                           dependencyContainerComponent.ResolveDependency<IIndexFactory>()));
            _components.Add(metadataConfigurationProvider);

		}

		private readonly IList<object> _components = new List<object>(); 

		public T GetComponent<T>() where T:class
		{
			return _components.FirstOrDefault(c => c is T) as T;
		}



	}
}
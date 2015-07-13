using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.AuthApi;
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

		private readonly Dictionary<Type, Lazy<object>> _components = new Dictionary<Type, Lazy<object>>();

		private void RegisterComponent<T>(Func<T> componentFactory) where T : class
		{
			// Компоненты регистрируются в виде ленивых зависимостей

			var component = new Lazy<object>(componentFactory);

			_components[typeof(T)] = component;
		}

		public T GetComponent<T>() where T : class
		{
			Lazy<object> component;

			if (_components.TryGetValue(typeof(T), out component))
			{
				return component.Value as T;
			}

			return default(T);
		}


		public GlobalContext(IDependencyContainerComponent container)
		{
			RegisterComponent(() => container);
			RegisterComponent(() => SystemComponent);

			RegisterComponent<IBlobStorageComponent>(() =>
			{
				var blobStorage = container.ResolveDependency<IBlobStorageFactory>().CreateBlobStorage();
				return new BlobStorageComponent(blobStorage);
			});

			RegisterComponent<IEventStorageComponent>(() =>
			{
				var eventStorage = container.ResolveDependency<IEventStorageFactory>().CreateEventStorage();
				return new EventStorageComponent(eventStorage);
			});

			RegisterComponent<IIndexComponent>(() =>
			{
				var indexFactory = container.ResolveDependency<IIndexFactory>();
				return new IndexComponent(indexFactory);
			});

			RegisterComponent<ILogComponent>(() =>
			{
				var logger = container.ResolveDependency<ILogFactory>().CreateLog();
				return new LogComponent(logger);
			});

			RegisterComponent<IMetadataComponent>(() =>
			{
				var metadataConfigurationProvider = container.ResolveDependency<IMetadataConfigurationProvider>();
				return new MetadataComponent(metadataConfigurationProvider);
			});

			RegisterComponent<ICrossConfigSearchComponent>(() =>
			{
				var crossConfigSearcher = container.ResolveDependency<ICrossConfigSearcher>();
				return new CrossConfigSearchComponent(crossConfigSearcher);
			});

			RegisterComponent<IPrintViewComponent>(() =>
			{
				var printViewBuilder = container.ResolveDependency<IPrintViewBuilderFactory>().CreatePrintViewBuilder();
				return new PrintViewComponent(printViewBuilder);
			});

			RegisterComponent<IProfilerComponent>(() =>
			{
				var logger = container.ResolveDependency<ILogFactory>().CreateLog();
				return new ProfilerComponent(logger);
			});

			RegisterComponent<IRegistryComponent>(() => new RegistryComponent());

			RegisterComponent<IScriptRunnerComponent>(() =>
			{
				var metadataConfigurationProvider = container.ResolveDependency<IMetadataConfigurationProvider>();
				return new ScriptRunnerComponent(metadataConfigurationProvider);
			});

			RegisterComponent<ISecurityComponent>(() => new SecurityComponent());

			RegisterComponent<ITransactionComponent>(() =>
			{
				var transactionManager = container.ResolveDependency<ITransactionManager>();
				return new TransactionComponent(transactionManager);
			});

			RegisterComponent<IWebClientNotificationComponent>(() =>
			{
				var webClientNotificationServiceFactory = container.ResolveDependency<IWebClientNotificationServiceFactory>();
				return new WebClientNotificationComponent(webClientNotificationServiceFactory);
			});

			RegisterComponent<IConfigurationMediatorComponent>(() =>
			{
				var configurationObjectBuilder = container.ResolveDependency<IConfigurationObjectBuilder>();
				var metadataConfigurationProvider = container.ResolveDependency<IMetadataConfigurationProvider>();
				return new ConfigurationMediatorComponent(configurationObjectBuilder, metadataConfigurationProvider);
			});

			RegisterComponent(() => new DocumentApi());
			RegisterComponent(() => new DocumentApiUnsecured());
			RegisterComponent(() => new PrintViewApi());
			RegisterComponent(() => new RegisterApi());
			RegisterComponent(() => new ReportApi());
			RegisterComponent(() => new UploadApi());
			RegisterComponent(() => new MetadataApi());
			RegisterComponent(() => new AclApi());
			RegisterComponent(() => new SignInApi());

			RegisterComponent<IPasswordVerifierComponent>(() => new PasswordVerifierComponent(this));

			RegisterComponent(() =>
			{
				var configurationObjectBuilder = container.ResolveDependency<IConfigurationObjectBuilder>();
				var metadataConfigurationProvider = container.ResolveDependency<IMetadataConfigurationProvider>();
				var configurationMediatorComponent = new ConfigurationMediatorComponent(configurationObjectBuilder, metadataConfigurationProvider);
				return new InprocessDocumentComponent(configurationMediatorComponent, new SecurityComponent());
			});
		}
	}
}
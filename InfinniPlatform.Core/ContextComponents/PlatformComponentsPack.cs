using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ApiContracts;
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
			_components.Add(new ContextRegistration(typeof(ILogComponent), () => new LogComponent(Logger.Log)));
			_components.Add(new ContextRegistration(typeof(IMetadataComponent),
				() => new MetadataComponent(metadataConfigurationProvider)));
			_components.Add(new ContextRegistration(typeof(ICrossConfigSearchComponent),
				() => new CrossConfigSearchComponent(dependencyContainerComponent.ResolveDependency<ICrossConfigSearcher>())));
			_components.Add(new ContextRegistration(typeof(IPrintViewComponent),
				() => new PrintViewComponent(printViewBuilder)));
			_components.Add(new ContextRegistration(typeof(IProfilerComponent),
				() => new ProfilerComponent(Logger.Log)));
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

			_components.Add(new ContextRegistration(typeof(IDocumentApi), () => new Api.RestApi.Public.DocumentApi()));
			_components.Add(new ContextRegistration(typeof(DocumentApiUnsecured), () => new DocumentApiUnsecured()));
			_components.Add(new ContextRegistration(typeof(IPrintViewApi), () => new Api.RestApi.Public.PrintViewApi()));
			_components.Add(new ContextRegistration(typeof(IRegisterApi), () => new Api.RestApi.Public.RegisterApi()));
			_components.Add(new ContextRegistration(typeof(IFileApi), () => new Api.RestApi.Public.FileApi()));
			_components.Add(new ContextRegistration(typeof(IAuthApi), () => new Api.RestApi.Public.AuthApi()));
			_components.Add(new ContextRegistration(typeof(ICustomServiceApi), () => new Api.RestApi.Public.CustomServiceApi()));

			_components.Add(new ContextRegistration(typeof(IApplicationUserManager), () =>
			{
				// Должен быть зарегистрирован при старте системы
				var hostingContext = dependencyContainerComponent.ResolveDependency<Hosting.IHostingContext>();

				return hostingContext.Get<IApplicationUserManager>();
			}));
		}

		public T GetComponent<T>() where T : class
		{
			return
				_components.Where(c => c.IsTypeOf(typeof(T))).Select(c => c.GetInstance()).FirstOrDefault() as T;
		}
	}
}
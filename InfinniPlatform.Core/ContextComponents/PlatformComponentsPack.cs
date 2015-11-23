using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestApi.Public;
using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.IoC;

using DocumentApi = InfinniPlatform.Api.RestApi.Public.DocumentApi;
using PrintViewApi = InfinniPlatform.Api.RestApi.Public.PrintViewApi;
using RegisterApi = InfinniPlatform.Api.RestApi.Public.RegisterApi;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Пакет компонентов платформы
    /// </summary>
    public sealed class PlatformComponentsPack : IPlatformComponentsPack
    {
        private static readonly ISystemComponent SystemComponent = new SystemComponent();

        public PlatformComponentsPack(Func<IContainerResolver> containerResolverFactory)
        {
            var containerResolver = containerResolverFactory();

            // TODO: Избавиться от этого кода после добавления IoC!!!
            var sessionManagerFactory = containerResolver.Resolve<ISessionManagerFactory>();
            var sessionManager = sessionManagerFactory.CreateSessionManager();

            var eventStorage = containerResolver.Resolve<IEventStorageFactory>().CreateEventStorage();
            var blobStorage = containerResolver.Resolve<IBlobStorageFactory>().CreateBlobStorage();
            var printViewBuilder = containerResolver.Resolve<IPrintViewBuilderFactory>().CreatePrintViewBuilder();
            var metadataConfigurationProvider = containerResolver.Resolve<IMetadataConfigurationProvider>();

            _components.Add(new ContextRegistration(typeof(IBlobStorageComponent),
                () => new BlobStorageComponent(blobStorage)));
            _components.Add(new ContextRegistration(typeof(IEventStorageComponent),
                () => new EventStorageComponent(eventStorage)));
            _components.Add(new ContextRegistration(typeof(IIndexComponent),
                () => new IndexComponent(containerResolver.Resolve<IIndexFactory>())));
            _components.Add(new ContextRegistration(typeof(ILogComponent), () => new LogComponent(Logger.Log)));
            _components.Add(new ContextRegistration(typeof(IMetadataComponent),
                () => new MetadataComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ICrossConfigSearchComponent),
                () => new CrossConfigSearchComponent(containerResolver.Resolve<ICrossConfigSearcher>())));
            _components.Add(new ContextRegistration(typeof(IPrintViewComponent),
                () => new PrintViewComponent(printViewBuilder)));
            _components.Add(new ContextRegistration(typeof(IProfilerComponent),
                () => new ProfilerComponent(Logger.Log)));
            _components.Add(new ContextRegistration(typeof(IRegistryComponent),
                () => new RegistryComponent()));
            _components.Add(new ContextRegistration(typeof(IScriptRunnerComponent),
                () => new ScriptRunnerComponent(metadataConfigurationProvider)));
            _components.Add(new ContextRegistration(typeof(ITransactionComponent),
                () =>
                    new TransactionComponent(containerResolver.Resolve<ITransactionManager>())));
            _components.Add(new ContextRegistration(typeof(IWebClientNotificationComponent),
                () =>
                    new WebClientNotificationComponent(
                        containerResolver.Resolve<IWebClientNotificationServiceFactory>())));
            _components.Add(new ContextRegistration(typeof(IConfigurationMediatorComponent),
                () => new ConfigurationMediatorComponent(
                    containerResolver.Resolve<IConfigurationObjectBuilder>())));

            _components.Add(new ContextRegistration(typeof(ISystemComponent), () => SystemComponent));

            _components.Add(new ContextRegistration(typeof(IMetadataConfigurationProvider),
                () => metadataConfigurationProvider));

            _components.Add(new ContextRegistration(typeof(IDocumentApi), () => new DocumentApi()));
            _components.Add(new ContextRegistration(typeof(DocumentApiUnsecured), () => new DocumentApiUnsecured()));
            _components.Add(new ContextRegistration(typeof(IPrintViewApi), () => new PrintViewApi()));
            _components.Add(new ContextRegistration(typeof(IRegisterApi), () => new RegisterApi()));
            _components.Add(new ContextRegistration(typeof(IFileApi), () => new FileApi()));
            _components.Add(new ContextRegistration(typeof(IAuthApi), () => new AuthApi()));
            _components.Add(new ContextRegistration(typeof(ICustomServiceApi), () => new CustomServiceApi()));

            _components.Add(new ContextRegistration(typeof(ISessionManager), () => sessionManager));

            _components.Add(new ContextRegistration(typeof(IApplicationUserManager), () => containerResolver.Resolve<IApplicationUserManager>()));
        }

        private readonly IList<ContextRegistration> _components = new List<ContextRegistration>();

        public T GetComponent<T>() where T : class
        {
            return _components.Where(c => c.IsTypeOf(typeof(T))).Select(c => c.GetInstance()).FirstOrDefault() as T;
        }
    }
}
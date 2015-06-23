using System.Collections.Generic;
using System.Linq;
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
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Factories
{
    /// <summary>
    ///  Реализация контекста компонентов платформы
    /// </summary>
    public class GlobalContext : IGlobalContext, IComponentContainer
    {
        
        private readonly IList<ContextRegistration> _components = new List<ContextRegistration>();
        private readonly IPlatformComponentsPack _platformComponentsPack;

        public GlobalContext(IDependencyContainerComponent dependencyContainerComponent)
        {
            _platformComponentsPack = new PlatformComponentsPack(dependencyContainerComponent);

            _components.Add(new ContextRegistration(typeof (DocumentApi), version => new DocumentApi(version)));
            _components.Add(new ContextRegistration(typeof (DocumentApiUnsecured),
                version => new DocumentApiUnsecured(version)));
            _components.Add(new ContextRegistration(typeof (PrintViewApi), version => new PrintViewApi(version)));
            _components.Add(new ContextRegistration(typeof (RegisterApi), version => new RegisterApi(version)));
            _components.Add(new ContextRegistration(typeof (ReportApi), version => new ReportApi(version)));
            _components.Add(new ContextRegistration(typeof (UploadApi), version => new UploadApi(version)));
            _components.Add(new ContextRegistration(typeof (MetadataApi), version => new MetadataApi(version)));
            _components.Add(new ContextRegistration(typeof (AuthApi), version => new AuthApi(version)));
            _components.Add(new ContextRegistration(typeof (SignInApi), version => new SignInApi(version)));
            _components.Add(new ContextRegistration(typeof (PasswordVerifierComponent),
                version => new PasswordVerifierComponent(this)));
            _components.Add(new ContextRegistration(typeof (InprocessDocumentComponent),
                version => new InprocessDocumentComponent(new ConfigurationMediatorComponent(
                    dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>()
                    ),
                    new CachedSecurityComponent(),
                    dependencyContainerComponent.ResolveDependency<IIndexFactory>())));
        }

        public T GetComponent<T>(string version) where T : class
        {
            //ищем среди зарегистрированных компонентов платформы, если не находим, обращаемся к контексту компонентов ядра платформы
            return
                _platformComponentsPack.GetComponent<T>(version) ??
                _components.Where(c => c.IsTypeOf(typeof (T))).Select(c => c.GetInstance(version)).FirstOrDefault() as T;
        }
    }
}
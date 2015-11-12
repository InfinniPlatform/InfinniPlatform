using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Factories
{
    /// <summary>
    /// Реализация контекста компонентов платформы
    /// </summary>
    public class GlobalContext : IGlobalContext, IComponentContainer
    {
        // TODO: Избавиться от этого кода после добавления IoC!!!
        private static ISessionManager _sessionManager;

        public GlobalContext(IDependencyContainerComponent dependencyContainerComponent)
        {
            // TODO: Избавиться от этого кода после добавления IoC!!!
            var sessionManagerFactory = dependencyContainerComponent.ResolveDependency<ISessionManagerFactory>();
            var sessionManager = sessionManagerFactory.CreateSessionManager();
            _sessionManager = sessionManager;

            _platformComponentsPack = new PlatformComponentsPack(dependencyContainerComponent);

            _components.Add(new ContextRegistration(typeof(ICustomServiceGlobalContext), dependencyContainerComponent.ResolveDependency<ICustomServiceGlobalContext>));
            _components.Add(new ContextRegistration(typeof(DocumentApi), () => new DocumentApi()));
            _components.Add(new ContextRegistration(typeof(DocumentApiUnsecured),
                () => new DocumentApiUnsecured()));
            _components.Add(new ContextRegistration(typeof(PrintViewApi), () => new PrintViewApi()));
            _components.Add(new ContextRegistration(typeof(RegisterApi), () => new RegisterApi()));
            _components.Add(new ContextRegistration(typeof(ReportApi), () => new ReportApi()));
            _components.Add(new ContextRegistration(typeof(UploadApi), () => new UploadApi()));
            _components.Add(new ContextRegistration(typeof(MetadataApi), () => new MetadataApi()));
            _components.Add(new ContextRegistration(typeof(AuthApi), () => new AuthApi()));
            _components.Add(new ContextRegistration(typeof(SignInApi), () => new SignInApi()));
            _components.Add(new ContextRegistration(typeof(InprocessDocumentComponent),
                () => new InprocessDocumentComponent(new ConfigurationMediatorComponent(
                    dependencyContainerComponent.ResolveDependency<IConfigurationObjectBuilder>()
                    ),
                    dependencyContainerComponent.ResolveDependency<IIndexFactory>())));

            _components.Add(new ContextRegistration(typeof(ISessionManager), () => sessionManager));

            _components.Add(new ContextRegistration(typeof(IApplicationUserManager), () =>
                                                                                     {
                                                                                         // Должен быть зарегистрирован при старте системы
                                                                                         var hostingContext = dependencyContainerComponent.ResolveDependency<IHostingContext>();

                                                                                         return hostingContext.Get<IApplicationUserManager>();
                                                                                     }));
        }

        private readonly IList<ContextRegistration> _components = new List<ContextRegistration>();
        private readonly IPlatformComponentsPack _platformComponentsPack;

        public T GetComponent<T>() where T : class
        {
            //ищем среди зарегистрированных компонентов платформы, если не находим, обращаемся к контексту компонентов ядра платформы
            return
                _platformComponentsPack.GetComponent<T>() ??
                _components.Where(c => c.IsTypeOf(typeof(T))).Select(c => c.GetInstance()).FirstOrDefault() as T;
        }

        public string GetVersion(string configuration, string userName)
        {
            var configVersions = GetComponent<IMetadataConfigurationProvider>().ConfigurationVersions;
            return GetComponent<IVersionStrategy>().GetActualVersion(configuration, configVersions, userName);
        }


        public static string GetTenantId()
        {
            string tenantId = null;

            var currentIdentity = GetCurrentIdentity();

            if (currentIdentity != null)
            {
                var sessionManager = _sessionManager;

                if (sessionManager != null)
                {
                    tenantId = sessionManager.GetSessionData(AuthorizationStorageExtensions.TenantId);
                }

                if (string.IsNullOrEmpty(tenantId))
                {
                    tenantId = currentIdentity.FindFirstClaim(AuthorizationStorageExtensions.DefaultTenantId);

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        tenantId = currentIdentity.FindFirstClaim(AuthorizationStorageExtensions.TenantId);
                    }
                }
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                tenantId = AuthorizationStorageExtensions.AnonimousUser;
            }

            return tenantId;
        }


        private static IIdentity GetCurrentIdentity()
        {
            var currentIdentity = Thread.CurrentPrincipal?.Identity;
            var currentUserId = currentIdentity?.FindFirstClaim(ClaimTypes.NameIdentifier);
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);
            return isNotAuthenticated ? null : currentIdentity;
        }
    }
}
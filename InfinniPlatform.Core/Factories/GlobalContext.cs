using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Factories
{
    /// <summary>
    /// Реализация контекста компонентов платформы
    /// </summary>
    public class GlobalContext : IGlobalContext, IComponentContainer
    {
        public GlobalContext(Func<IContainerResolver> containerResolverFactory, ISessionManager sessionManager)
        {
            _containerResolver = containerResolverFactory();
            _sessionManager = sessionManager;
        }


        private readonly IContainerResolver _containerResolver;


        [Obsolete("Use IoC")]
        public T GetComponent<T>() where T : class
        {
            return _containerResolver.Resolve<T>();
        }


        // TODO: Избавиться от нижележащего кода после добавления IoC!!!

        private static ISessionManager _sessionManager;

        private static readonly string[] SystemConfigurations =
        {
            "administration",
            "administrationcustomization",
            "authorization",
            "restfulapi",
            "systemconfig",
            "update"
        };

        public static string GetTenantId(string indexName = null)
        {
            string tenantId = null;

            if (indexName != null && SystemConfigurations.Contains(indexName, StringComparer.OrdinalIgnoreCase))
            {
                tenantId = AuthorizationStorageExtensions.AnonymousUser;
            }
            else
            {
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
                    tenantId = AuthorizationStorageExtensions.AnonymousUser;
                }
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
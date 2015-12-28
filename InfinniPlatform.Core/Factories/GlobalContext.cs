using System;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Transactions;

namespace InfinniPlatform.Factories
{
    /// <summary>
    /// Реализация контекста компонентов платформы
    /// </summary>
    public class GlobalContext : IGlobalContext, IComponentContainer
    {
        public GlobalContext(IContainerResolver containerResolver, ITenantProvider tenantProvider)
        {
            _containerResolver = containerResolver;
            _tenantProvider = tenantProvider;
        }


        private readonly IContainerResolver _containerResolver;


        [Obsolete("Use IoC")]
        public T GetComponent<T>() where T : class
        {
            return _containerResolver.Resolve<T>();
        }


        private static ITenantProvider _tenantProvider;

        [Obsolete("Use ITenantProvider")]
        public static string GetTenantId(string indexName = null)
        {
            return _tenantProvider?.GetTenantId(indexName);
        }
    }
}
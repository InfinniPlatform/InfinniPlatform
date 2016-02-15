using System;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageFilterProvider : IDocumentStorageFilterProvider
    {
        private readonly ITenantProvider _tenantProvider;

        public DocumentStorageFilterProvider(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            return f =>
                   {
                       var tenants = new[] { _tenantProvider.GetTenantId(), DocumentStorageHelpers.AnonymousUser };

                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.In("_header._tenant", tenants), f.Exists("_header._deleted", false));

                       return (appFilter != null) ? f.And(appFilter, systemFilter) : systemFilter;
                   };
        }
    }
}
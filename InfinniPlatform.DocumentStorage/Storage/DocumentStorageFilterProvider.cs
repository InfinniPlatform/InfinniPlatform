using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal sealed class DocumentStorageFilterProvider : IDocumentStorageFilterProvider
    {
        public DocumentStorageFilterProvider(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }


        private readonly ITenantProvider _tenantProvider;


        public Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            var tenants = GetAvailableTenants();

            return f =>
                   {
                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.In("_header._tenant", tenants), f.Exists("_header._deleted", false));

                       return (appFilter != null) ? f.And(appFilter, systemFilter) : systemFilter;
                   };
        }

        public Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document
        {
            var tenants = GetAvailableTenants();

            Expression<Func<TDocument, bool>> systemFilter = i => tenants.Contains(i._header._tenant) && i._header._deleted == null;

            return filter.And(systemFilter);
        }


        private string[] GetAvailableTenants()
        {
            return new[] { _tenantProvider.GetTenantId(), DocumentStorageHelpers.AnonymousUser };
        }
    }
}
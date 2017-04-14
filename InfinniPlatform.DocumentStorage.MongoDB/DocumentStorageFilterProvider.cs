using System;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class DocumentStorageFilterProvider : IDocumentStorageFilterProvider
    {
        public DocumentStorageFilterProvider(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }


        private readonly ITenantProvider _tenantProvider;


        public Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            return f =>
                   {
                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.Eq("_header._tenant", _tenantProvider.GetTenantId()), f.Exists("_header._deleted", false));

                       return appFilter != null ? f.And(appFilter, systemFilter) : systemFilter;
                   };
        }

        public Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document
        {
            Expression<Func<TDocument, bool>> systemFilter = i => _tenantProvider.GetTenantId() == i._header._tenant && i._header._deleted == null;

            return filter.And(systemFilter);
        }
    }
}
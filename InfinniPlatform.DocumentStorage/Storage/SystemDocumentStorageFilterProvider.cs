using System;
using System.Linq.Expressions;

using InfinniPlatform.DocumentStorage.Contract;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemDocumentStorageFilterProvider : ISystemDocumentStorageFilterProvider
    {
        public SystemDocumentStorageFilterProvider(ISystemTenantProvider systemTenantProvider)
        {
            _systemTenantProvider = systemTenantProvider;
        }

        private readonly ISystemTenantProvider _systemTenantProvider;

        public Func<IDocumentFilterBuilder, object> AddSystemFilter(Func<IDocumentFilterBuilder, object> filter)
        {
            var systemTenantId = _systemTenantProvider.GetTenantId();

            return f =>
                   {
                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.Eq("_header._tenant", systemTenantId), f.Exists("_header._deleted", false));

                       return (appFilter != null) ? f.And(appFilter, systemFilter) : systemFilter;
                   };
        }

        public Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document
        {
            var systemTenantId = _systemTenantProvider.GetTenantId();

            Expression<Func<TDocument, bool>> systemFilter = i => i._header._tenant == systemTenantId && i._header._deleted == null;

            return filter.And(systemFilter);
        }
    }
}
using System;
using System.Linq.Expressions;

using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Documents;

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
            return f =>
                   {
                       var appFilter = filter?.Invoke(f);
                       var systemFilter = f.And(f.Eq("_header._tenant", _systemTenantProvider.GetTenantId()), f.Exists("_header._deleted", false));

                       return appFilter != null
                                  ? f.And(appFilter, systemFilter)
                                  : systemFilter;
                   };
        }

        public Expression<Func<TDocument, bool>> AddSystemFilter<TDocument>(Expression<Func<TDocument, bool>> filter) where TDocument : Document
        {
            Expression<Func<TDocument, bool>> systemFilter = i => i._header._tenant == _systemTenantProvider.GetTenantId() && i._header._deleted == null;

            return filter != null
                       ? DocumentStorageFilterProvider.LambdaExpressionComposer.Compose(filter, systemFilter, Expression.AndAlso)
                       : systemFilter;
        }
    }
}
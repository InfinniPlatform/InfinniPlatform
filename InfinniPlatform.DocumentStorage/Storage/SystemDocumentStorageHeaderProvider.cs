using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemDocumentStorageHeaderProvider : DocumentStorageHeaderProvider, ISystemDocumentStorageHeaderProvider
    {
        public SystemDocumentStorageHeaderProvider(ISystemTenantProvider tenantProvider,
                                                   IUserIdentityProvider userIdentityProvider)
            : base(tenantProvider, userIdentityProvider)
        {
        }
    }
}
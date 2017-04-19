using InfinniPlatform.Core.Security;

namespace InfinniPlatform.DocumentStorage.MongoDB
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
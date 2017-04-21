using InfinniPlatform.Security;

namespace InfinniPlatform.DocumentStorage
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
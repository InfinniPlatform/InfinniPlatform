using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.DocumentStorage
{
    internal class SystemDocumentStorageHeaderProvider : DocumentStorageHeaderProvider, ISystemDocumentStorageHeaderProvider
    {
        public SystemDocumentStorageHeaderProvider(ISystemTenantProvider tenantProvider,
                                                   IHttpContextAccessor httpContextAccessor)
            : base(tenantProvider, httpContextAccessor)
        {
        }
    }
}
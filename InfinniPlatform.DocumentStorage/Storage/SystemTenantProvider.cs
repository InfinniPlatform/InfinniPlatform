using System.Security.Principal;

using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemTenantProvider : ISystemTenantProvider
    {
        public string GetTenantId()
        {
            return SecurityConstants.SystemUserTenantId;
        }

        public string GetTenantId(IIdentity identity)
        {
            return SecurityConstants.SystemUserTenantId;
        }
    }
}
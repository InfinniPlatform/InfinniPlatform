using System.Security.Principal;

using InfinniPlatform.Security;

namespace InfinniPlatform.DocumentStorage
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
using System.Security.Principal;

namespace InfinniPlatform.DocumentStorage.Storage
{
    internal class SystemTenantProvider : ISystemTenantProvider
    {
        private const string SystemTenant = "system";

        public string GetTenantId()
        {
            return SystemTenant;
        }

        public string GetTenantId(IIdentity identity)
        {
            return SystemTenant;
        }
    }
}
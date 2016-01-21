using System.Security.Principal;

using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.Core.Transactions
{
    /// <summary>
    /// Предоставляет методы определения пользователя системы по модели SaaS.
    /// </summary>
    internal class TenantProvider : ITenantProvider
    {
        private const string TenantId = "tenantid";
        private const string AnonymousUser = "anonimous";
        private const string DefaultTenantId = "defaulttenantid";


        public TenantProvider(ISessionManager sessionManager, IUserIdentityProvider userIdentityProvider)
        {
            _sessionManager = sessionManager;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly ISessionManager _sessionManager;
        private readonly IUserIdentityProvider _userIdentityProvider;


        public string GetTenantId()
        {
            string tenantId = null;

            var currentIdentity = GetCurrentIdentity();

            if (currentIdentity != null)
            {
                var sessionManager = _sessionManager;

                if (sessionManager != null)
                {
                    tenantId = sessionManager.GetSessionData(TenantId);
                }

                if (string.IsNullOrEmpty(tenantId))
                {
                    tenantId = currentIdentity.FindFirstClaim(DefaultTenantId);

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        tenantId = currentIdentity.FindFirstClaim(TenantId);
                    }
                }
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                tenantId = AnonymousUser;
            }

            return tenantId;
        }


        private IIdentity GetCurrentIdentity()
        {
            var currentIdentity = _userIdentityProvider.GetCurrentUserIdentity();
            var currentUserId = currentIdentity.GetUserId();
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);
            return isNotAuthenticated ? null : currentIdentity;
        }
    }
}
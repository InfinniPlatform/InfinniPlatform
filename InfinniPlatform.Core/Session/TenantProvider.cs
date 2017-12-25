using System.Security.Principal;

using InfinniPlatform.Security;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Session
{
    /// <summary>
    /// Предоставляет методы определения пользователя системы по модели SaaS.
    /// </summary>
    internal class TenantProvider : ITenantProvider
    {
        public TenantProvider(ITenantScopeProvider tenantScopeProvider,
                              IHttpContextAccessor httpContextAccessor)
        {
            _tenantScopeProvider = tenantScopeProvider;
            _httpContextAccessor = httpContextAccessor;
        }


        private readonly ITenantScopeProvider _tenantScopeProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public string GetTenantId()
        {
            var currentIdentity = GetCurrentIdentity();

            return GetTenantId(currentIdentity);
        }

        public string GetTenantId(IIdentity identity)
        {
            string tenantId;

            var tenantScope = _tenantScopeProvider.GetTenantScope();

            if (tenantScope != null)
            {
                tenantId = tenantScope.TenantId;
            }
            else if (identity != null && identity.IsAuthenticated)
            {
                // Идентификатор текущей организации
                tenantId = identity.FindFirstClaim(AppClaimTypes.TenantId);

                if (string.IsNullOrEmpty(tenantId))
                {
                    // Идентификатор организации по умолчанию
                    tenantId = identity.FindFirstClaim(AppClaimTypes.DefaultTenantId);

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        // Организация не определена
                        tenantId = TenantIdConstants.UndefinedUserTenantId;
                    }
                }
            }
            else
            {
                // Анонимный пользователь
                tenantId = TenantIdConstants.AnonymousUserTenantId;
            }

            return tenantId;
        }


        private IIdentity GetCurrentIdentity()
        {
            var currentIdentity = _httpContextAccessor.HttpContext.User.Identity;
            var currentUserId = currentIdentity.GetUserId();
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);
            return isNotAuthenticated ? null : currentIdentity;
        }
    }
}
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Security
{
    internal class UserIdentityProvider : IUserIdentityProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentityProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IIdentity Get()
        {
            return _httpContextAccessor.HttpContext.User.Identity;
        }
    }
}
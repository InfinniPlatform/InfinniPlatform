using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserAuthenticationTokenStore<TUser> : UserStore<TUser>, IUserAuthenticationTokenStore<TUser>
        where TUser : AppUser
    {
        public UserAuthenticationTokenStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache) : base(documentStorageFactory, userCache)
        {
        }

        public virtual Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken token)
        {
            return Task.Run(() => user.SetToken(loginProvider, name, value), token);
        }

        public virtual Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            return Task.Run(() => user.RemoveToken(loginProvider, name), token);
        }

        public virtual Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            return Task.Run(() => user.GetTokenValue(loginProvider, name), token);
        }
    }
}
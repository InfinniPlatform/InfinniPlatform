using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserLoginStore<TUser> : UserStore<TUser>, IUserLoginStore<TUser> where TUser : AppUser
    {
        public UserLoginStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            user.AddLogin(login);
            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public virtual async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token)
        {
            user.RemoveLogin(loginProvider, providerKey);
            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.Logins.Select(l => l.ToUserLoginInfo()) as IList<UserLoginInfo>;
        }

        public virtual Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByLogin(loginProvider, providerKey),
                                   async () => await Users.Value.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).FirstOrDefaultAsync());
        }
    }
}
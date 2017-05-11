using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public partial class UserStore<TUser> : IUserLoginStore<TUser> where TUser : AppUser
    {
        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            user.AddLogin(login);

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token)
        {
            user.RemoveLogin(loginProvider, providerKey);

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Logins.Select(l => l.ToUserLoginInfo()) as IList<UserLoginInfo>);
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByLogin(loginProvider, providerKey),
                                   async () => await Users.Value.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).FirstOrDefaultAsync());
        }
    }
}
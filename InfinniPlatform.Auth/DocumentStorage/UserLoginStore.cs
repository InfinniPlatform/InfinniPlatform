using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserLoginStore<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            user.AddLogin(login);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token)
        {
            user.RemoveLogin(loginProvider, providerKey);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Logins.Select(l => l.ToUserLoginInfo()) as IList<UserLoginInfo>);
        }

        /// <inheritdoc />
        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) _userCache.FindUserByLogin(loginProvider, providerKey),
                                   async () => await _users.Value.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).FirstOrDefaultAsync());
        }
    }
}
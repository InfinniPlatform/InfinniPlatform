﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserLoginStore<TUser> where TUser : AppUser
    {
        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            return Task.Run(() => user.AddLogin(login), token);
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token)
        {
            return Task.Run(() => user.RemoveLogin(loginProvider, providerKey), token);
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
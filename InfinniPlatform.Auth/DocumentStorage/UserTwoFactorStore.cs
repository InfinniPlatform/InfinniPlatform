﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserTwoFactorStore<TUser> where TUser : AppUser
    {
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            return Task.Run(() => user.TwoFactorEnabled = enabled, token);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
    }
}
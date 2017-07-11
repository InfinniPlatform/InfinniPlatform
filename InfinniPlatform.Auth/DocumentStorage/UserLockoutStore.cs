using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserLockoutStore<TUser> where TUser : AppUser
    {
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.GetValueOrDefault() : new DateTimeOffset?());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            return Task.Run(() => user.LockoutEndDateUtc = lockoutEnd.HasValue ? lockoutEnd.GetValueOrDefault().UtcDateTime : new DateTime?(), token);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() =>
            {
                ++user.AccessFailedCount;
                return user.AccessFailedCount;
            }, token);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() => user.AccessFailedCount = 0, token);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            return Task.Run(() => user.LockoutEnabled = enabled, token);
        }
    }
}
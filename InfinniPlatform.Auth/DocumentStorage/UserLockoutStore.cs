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

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            user.LockoutEndDateUtc = lockoutEnd.HasValue ? lockoutEnd.GetValueOrDefault().UtcDateTime : new DateTime?();

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            ++user.AccessFailedCount;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);

            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            user.AccessFailedCount = 0;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            user.LockoutEnabled = enabled;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }
    }
}
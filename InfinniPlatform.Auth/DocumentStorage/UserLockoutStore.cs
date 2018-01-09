using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserLockoutStore<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.GetValueOrDefault() : new DateTimeOffset?());
        }

        /// <inheritdoc />
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            user.LockoutEndDateUtc = lockoutEnd.HasValue
                                         ? lockoutEnd.GetValueOrDefault().UtcDateTime
                                         : new DateTime?();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            ++user.AccessFailedCount;
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <inheritdoc />
        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            user.AccessFailedCount = 0;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <inheritdoc />
        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <inheritdoc />
        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            user.LockoutEnabled = enabled;

            return Task.CompletedTask;
        }
    }
}
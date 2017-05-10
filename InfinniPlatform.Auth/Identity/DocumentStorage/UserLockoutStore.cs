using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserLockoutStore<TUser> : UserStore<TUser>, IUserLockoutStore<TUser> where TUser : AppUser
    {
        public UserLockoutStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.LockoutEndDateUtc.HasValue ? storedUser.LockoutEndDateUtc.GetValueOrDefault() : new DateTimeOffset?();
        }

        public virtual async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            user.LockoutEndDateUtc = lockoutEnd.HasValue ? lockoutEnd.GetValueOrDefault().UtcDateTime : new DateTime?();
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            ++user.AccessFailedCount;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);

            return user.AccessFailedCount;
        }

        public virtual async Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            user.AccessFailedCount = 0;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.AccessFailedCount;
        }

        public virtual async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.LockoutEnabled;
        }

        public virtual async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            user.LockoutEnabled = enabled;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }
    }
}
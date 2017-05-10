using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserEmailStore<TUser> : UserStore<TUser>, IUserEmailStore<TUser> where TUser : AppUser
    {
        public UserEmailStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.EmailConfirmed;
        }

        public virtual async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.EmailConfirmed = confirmed;

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public virtual async Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            user.Email = email;

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public virtual async Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.Email;
        }

        public virtual async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.NormalizedEmail;
        }

        public virtual async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken token)
        {
            user.NormalizedEmail = normalizedEmail;

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public virtual Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByEmail(normalizedEmail),
                                   async () => await Users.Value.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync());
        }
    }
}
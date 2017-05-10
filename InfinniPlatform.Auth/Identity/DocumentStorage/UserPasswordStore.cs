using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserPasswordStore<TUser> : UserStore<TUser>, IUserPasswordStore<TUser> where TUser : AppUser
    {
        public UserPasswordStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }


        public virtual async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            user.PasswordHash = passwordHash;
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.PasswordHash;
        }

        public virtual async Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.HasPassword();
        }
    }
}
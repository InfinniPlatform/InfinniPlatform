using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserPhoneNumberStore<TUser> : UserStore<TUser>, IUserPhoneNumberStore<TUser> where TUser : AppUser
    {
        public UserPhoneNumberStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            user.PhoneNumber = phoneNumber;
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.PhoneNumber;
        }

        public virtual async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.PhoneNumberConfirmed;
        }

        public virtual async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.PhoneNumberConfirmed = confirmed;
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }
    }
}
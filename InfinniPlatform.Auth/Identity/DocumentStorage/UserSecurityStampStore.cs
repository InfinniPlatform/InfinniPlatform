using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserSecurityStampStore<TUser> : UserStore<TUser>, IUserSecurityStampStore<TUser> where TUser : AppUser
    {
        public UserSecurityStampStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken token)
        {
            user.SecurityStamp = stamp;
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<string> GetSecurityStampAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.SecurityStamp;
        }
    }
}
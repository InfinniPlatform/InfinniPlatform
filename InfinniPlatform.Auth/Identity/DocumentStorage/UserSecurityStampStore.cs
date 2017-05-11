using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public partial class UserStore<TUser> : IUserSecurityStampStore<TUser> where TUser : AppUser
    {
        public async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken token)
        {
            user.SecurityStamp = stamp;
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}
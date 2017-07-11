using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserSecurityStampStore<TUser> where TUser : AppUser
    {
        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken token)
        {
            return Task.Run(() => user.SecurityStamp = stamp, token);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}
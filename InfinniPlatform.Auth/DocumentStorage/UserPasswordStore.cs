using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserPasswordStore<TUser> where TUser : AppUser
    {
        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            return Task.Run(() => user.PasswordHash = passwordHash, token);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.HasPassword());
        }
    }
}
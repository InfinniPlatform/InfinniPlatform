using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserPasswordStore<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <inheritdoc />
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.HasPassword());
        }
    }
}
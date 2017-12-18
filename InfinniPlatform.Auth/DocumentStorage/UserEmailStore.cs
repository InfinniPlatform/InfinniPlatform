using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserEmailStore<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <inheritdoc />
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            user.Email = email;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Email);
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        /// <inheritdoc />
        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken token)
        {
            user.NormalizedEmail = normalizedEmail;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByEmail(normalizedEmail),
                                   async () => await Users.Value.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync());
        }
    }
}
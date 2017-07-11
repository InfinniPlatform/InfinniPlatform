using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserEmailStore<TUser> where TUser : AppUser
    {
        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            return Task.Run(()=> user.EmailConfirmed = confirmed, token);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            return Task.Run(() => user.Email = email, token);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Email);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken token)
        {
            return Task.Run(() => user.NormalizedEmail = normalizedEmail, token);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByEmail(normalizedEmail),
                                   async () => await Users.Value.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync());
        }
    }
}
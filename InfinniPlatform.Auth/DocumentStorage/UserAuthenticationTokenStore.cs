using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserAuthenticationTokenStore<TUser> where TUser : AppUser
    {
        public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken token)
        {
            user.SetToken(loginProvider, name, value);

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            user.RemoveToken(loginProvider, name);

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            return Task.FromResult(user.GetTokenValue(loginProvider, name));
        }
    }
}
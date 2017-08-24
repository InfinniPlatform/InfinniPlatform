using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserRoleStore<TUser> where TUser : AppUser
    {
        public Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.AddRole(normalizedRoleName);

            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.RemoveRole(normalizedRoleName);

            return Task.CompletedTask;
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Roles as IList<string>);
        }

        public Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            return Task.FromResult(user.Roles.Contains(normalizedRoleName));
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken token)
        {
            return await Users.Value.Find(u => u.Roles.Contains(normalizedRoleName)).ToListAsync();
        }
    }
}
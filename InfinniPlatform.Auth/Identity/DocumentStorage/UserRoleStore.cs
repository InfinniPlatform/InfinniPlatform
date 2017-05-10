using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserRoleStore<TUser> : UserStore<TUser>, IUserRoleStore<TUser> where TUser : AppUser
    {
        public UserRoleStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
            : base(documentStorageFactory, userCache)
        {
        }

        public virtual async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.AddRole(normalizedRoleName);
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.RemoveRole(normalizedRoleName);
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public virtual async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.Roles;
        }

        public virtual async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.Roles.Contains(normalizedRoleName);
        }

        public virtual async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken token)
        {
            return await Users.Value.Find(u => u.Roles.Contains(normalizedRoleName)).ToListAsync();
        }
    }
}
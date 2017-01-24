using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace InfinniPlatform.Auth.Internal.Identity.MongoDb
{
    public class RoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : IdentityRole
    {
        private readonly IMongoCollection<TRole> _roles;

        public RoleStore(IMongoCollection<TRole> roles)
        {
            _roles = roles;
        }

        public virtual IQueryable<TRole> Roles => _roles.AsQueryable();

        public virtual void Dispose()
        {
        }

        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
        {
            await _roles.InsertOneAsync(role, null, token);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            var replaceOneResult = await _roles.ReplaceOneAsync(r => r.Id == role.Id, role, null, token);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            var deleteResult = await _roles.DeleteOneAsync(r => r.Id == role.Id, token);
            return IdentityResult.Success;
        }

        public virtual async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return role.Id;
        }

        public virtual async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return role.Name;
        }

        public virtual async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
        }

        public virtual async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return role.NormalizedName;
        }

        public virtual async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
        }

        public virtual Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
        {
            return _roles.Find(r => r.Id == roleId, null).FirstOrDefaultAsync(token);
        }

        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
        {
            return _roles.Find(r => r.NormalizedName == normalizedName, null).FirstOrDefaultAsync(token);
        }
    }
}
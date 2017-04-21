using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage;

using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.MongoDb
{
    public class RoleStore<TRole> : IQueryableRoleStore<TRole> where TRole : IdentityRole
    {
        private readonly IDocumentStorage<TRole> _roles;

        public RoleStore(IDocumentStorage<TRole> roles)
        {
            _roles = roles;
        }

        public virtual IQueryable<TRole> Roles => _roles.Find().ToList().AsQueryable();

        public virtual void Dispose()
        {
        }

        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
        {
            await _roles.InsertOneAsync(role);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            var replaceOneResult = await _roles.ReplaceOneAsync(role, r => r.Id == role.Id);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            var deleteResult = await _roles.DeleteOneAsync(r => r.Id == role.Id);
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
            return _roles.Find(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
        {
            return _roles.Find(r => r.NormalizedName == normalizedName).FirstOrDefaultAsync();
        }
    }
}
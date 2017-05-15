using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public class RoleStore<TRole> : IRoleStore<TRole> where TRole : AppUserRole
    {
        private readonly Lazy<ISystemDocumentStorage<TRole>> _roles;

        public RoleStore(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _roles = new Lazy<ISystemDocumentStorage<TRole>>(() => documentStorageFactory.GetStorage<TRole>());
        }

        public virtual void Dispose()
        {
        }

        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.InsertOneAsync(role);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.ReplaceOneAsync(role, r => r.Id == role.Id);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.DeleteOneAsync(r => r.Id == role.Id);
            return IdentityResult.Success;
        }

        public virtual Task<string> GetRoleIdAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.Id);
        }

        public virtual Task<string> GetRoleNameAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.Name);
        }

        public virtual Task SetRoleNameAsync(TRole role, string roleName, CancellationToken token)
        {
            return Task.Run(() => role.Name = roleName, token);
        }

        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken token)
        {
            return Task.Run(() => role.NormalizedName = normalizedName, token);
        }

        public virtual Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
        {
            return _roles.Value.Find(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
        {
            return _roles.Value.Find(r => r.NormalizedName == normalizedName).FirstOrDefaultAsync();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    /// <inheritdoc />
    public class RoleStore<TRole> : IRoleStore<TRole> where TRole : AppUserRole
    {
        private readonly Lazy<ISystemDocumentStorage<TRole>> _roles;

        /// <summary>
        /// Initializes a new instance of <see cref="RoleStore{TRole}" />.
        /// </summary>
        /// <param name="documentStorageFactory">Roles database storage.</param>
        public RoleStore(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _roles = new Lazy<ISystemDocumentStorage<TRole>>(() => documentStorageFactory.GetStorage<TRole>());
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
        }

        /// <inheritdoc />
        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.InsertOneAsync(role);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.ReplaceOneAsync(role, r => r.Id == role.Id);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            await _roles.Value.DeleteOneAsync(r => r.Id == role.Id);
            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public virtual Task<string> GetRoleIdAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.Id);
        }

        /// <inheritdoc />
        public virtual Task<string> GetRoleNameAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.Name);
        }

        /// <inheritdoc />
        public virtual Task SetRoleNameAsync(TRole role, string roleName, CancellationToken token)
        {
            role.Name = roleName;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken token)
        {
            return Task.FromResult(role.NormalizedName);
        }

        /// <inheritdoc />
        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken token)
        {
            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
        {
            return _roles.Value.Find(r => r.Id == roleId).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
        {
            return _roles.Value.Find(r => r.NormalizedName == normalizedName).FirstOrDefaultAsync();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    /// <summary>
    /// User store.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    public partial class UserStore<TUser> : IUserStore<TUser> where TUser : AppUser
    {
        /// <summary>
        /// Local application cache of users.
        /// </summary>
        protected readonly UserCache<AppUser> UserCache;

        /// <summary>
        /// Users database storage.
        /// </summary>
        protected readonly Lazy<ISystemDocumentStorage<TUser>> Users;

        /// <summary>
        /// Initializes a new instance of <see cref="UserStore{TUser}" />.
        /// </summary>
        /// <param name="documentStorageFactory">Users database storage.</param>
        /// <param name="userCache">Local application cache of users.</param>
        public UserStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
        {
            Users = new Lazy<ISystemDocumentStorage<TUser>>(() => documentStorageFactory.GetStorage<TUser>());
            UserCache = userCache;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.InsertOneAsync(user);

            UpdateUserInCache(user);

            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.ReplaceOneAsync(user, u => u._id.Equals(user._id));

            UpdateUserInCache(user);

            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await Users.Value.DeleteOneAsync(u => u._id.Equals(user._id));

            RemoveUserFromCache(user._id.ToString());

            return IdentityResult.Success;
        }

        /// <inheritdoc />
        public Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user._id.ToString());
        }

        /// <inheritdoc />
        public Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.UserName);
        }

        /// <inheritdoc />
        public Task SetUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        /// <inheritdoc />
        public Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken token)
        {
            user.NormalizedUserName = normalizedUserName;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserById(userId),
                                         async () => await Users.Value.Find(u => u._id.Equals(userId)).FirstOrDefaultAsync());
        }

        /// <inheritdoc />
        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserByUserName(normalizedUserName),
                                         async () => await Users.Value.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync());
        }

        /// <summary>
        /// Updates user information in local application cache.
        /// </summary>
        protected void UpdateUserInCache(AppUser user)
        {
            UserCache.AddOrUpdateUser(user);
        }

        /// <summary>
        /// Removes user information in local application cache.
        /// </summary>
        protected void RemoveUserFromCache(string userId)
        {
            UserCache.RemoveUser(userId);
        }

        /// <summary>
        /// Finds user informations in local application cache.
        /// </summary>
        protected async Task<TUser> FindUserInCache(Func<TUser> cacheSelector, Func<Task<TUser>> storageSelector)
        {
            var user = cacheSelector();

            if (user == null)
            {
                user = await storageSelector();

                if (user != null)
                {
                    UserCache.AddOrUpdateUser(user);
                }
            }

            return user;
        }
    }
}
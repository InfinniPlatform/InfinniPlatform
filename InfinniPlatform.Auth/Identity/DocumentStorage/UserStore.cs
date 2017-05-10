using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    /// <typeparam name="TUser">Пользователь.</typeparam>
    public class UserStore<TUser> : IUserStore<TUser> where TUser : AppUser
    {
        protected readonly UserCache<AppUser> UserCache;
        protected readonly Lazy<ISystemDocumentStorage<TUser>> Users;

        public UserStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
        {
            Users = new Lazy<ISystemDocumentStorage<TUser>>(() => documentStorageFactory.GetStorage<TUser>());
            UserCache = userCache;
        }

        public virtual void Dispose()
        {
        }

        public virtual async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.InsertOneAsync(user);
            UpdateUserInCache(user);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await Users.Value.DeleteOneAsync(u => u.Id == user.Id);
            RemoveUserFromCache(user.Id);
            return IdentityResult.Success;
        }

        public virtual async Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.NormalizedUserName).FirstOrDefaultAsync();

            return storedUser.Id;
        }

        public virtual async Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.UserName;
        }

        public virtual async Task SetUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            await Users.Value.UpdateOneAsync(builder => builder.Set(u => u.UserName, userName), u => u.Id == user.Id);
        }

        public virtual async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken token)
        {
            var storedUser = await Users.Value.Find(u => u.Id == user.Id).FirstOrDefaultAsync();

            return storedUser.NormalizedUserName;
        }

        public virtual async Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken token)
        {
            await Users.Value.UpdateOneAsync(builder => builder.Set(u => u.NormalizedUserName, normalizedUserName), u => u.Id == user.Id);
        }

        public virtual async Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserById(userId),
                                         async () => await Users.Value.Find(u => u._id.Equals(userId)).FirstOrDefaultAsync());
        }

        public virtual async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserByUserName(normalizedUserName),
                                         async () => await Users.Value.Find(u => u.UserName == normalizedUserName.ToLower()).FirstOrDefaultAsync());
        }

        /// <summary>
        /// Обновляет сведения о пользователе в локальном кэше.
        /// </summary>
        protected void UpdateUserInCache(AppUser user)
        {
            UserCache.AddOrUpdateUser(user);
        }

        /// <summary>
        /// Удаляет сведения о пользователе из локального кэша.
        /// </summary>
        protected void RemoveUserFromCache(string userId)
        {
            UserCache.RemoveUser(userId);
        }

        /// <summary>
        /// Ищет сведения о пользователе в локальном кэше.
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
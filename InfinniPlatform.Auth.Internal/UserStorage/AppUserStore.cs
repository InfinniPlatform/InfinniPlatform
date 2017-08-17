using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using InfinniPlatform.Auth.Internal.Properties;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Auth.Internal.UserStorage
{
    internal class AppUserStore : IAppUserStore
    {
        public AppUserStore(Lazy<AppUserStoreCache> userCache,
                            ISystemDocumentStorageFactory documentStorageFactory)
        {
            // Lazy, чтобы подписка на изменения кэша пользователей в кластере не создавалась сразу

            _userCache = userCache;
            _userStorage = new Lazy<ISystemDocumentStorage<ApplicationUser>>(() => documentStorageFactory.GetStorage<ApplicationUser>());
        }


        private readonly Lazy<AppUserStoreCache> _userCache;
        private readonly Lazy<ISystemDocumentStorage<ApplicationUser>> _userStorage;


        public async Task CreateUserAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = CreateUnique();
            }

            await UpdateUserAsync(user);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await SaveUser(user);
            UpdateUserInCache(user);
        }

        public async Task DeleteUserAsync(ApplicationUser user)
        {
            await _userStorage.Value.DeleteOneAsync(f => f._id == user._id);

            RemoveUserFromCache(user.Id);
        }

        public async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return await FindUserInCacheAsync(c => c.FindUserById(userId), async () => await FindUser(f => f._header._deleted == null && f.Id == userId));
        }

        public async Task<ApplicationUser> FindUserByUserNameAsync(string userName)
        {
            return await FindUserInCacheAsync(c => c.FindUserByUserName(userName), async () => await FindUser(f => f._header._deleted == null && f.UserName == userName));
        }

        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            return await FindUserInCacheAsync(c => c.FindUserByEmail(email), async () => await FindUser(f => f._header._deleted == null && f.Email == email));
        }

        public async Task<ApplicationUser> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            return await FindUserInCacheAsync(c => c.FindUserByPhoneNumber(phoneNumber), async () => await FindUser(f => f._header._deleted == null && f.PhoneNumber == phoneNumber));
        }

        public async Task<ApplicationUser> FindUserByLoginAsync(ApplicationUserLogin userLogin)
        {
            return await FindUserInCacheAsync(c => c.FindUserByLogin(userLogin), async () => await FindUser(f => f._header._deleted == null && f.Logins.Any(l => l.ProviderKey == userLogin.ProviderKey)));
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return await FindUserByUserNameAsync(name) ?? await FindUserByEmailAsync(name) ?? await FindUserByPhoneNumberAsync(name);
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Resources.CantAddUnsavedUserToRole);
            }

            var roles = user.Roles.ToList();

            if (roles.All(r => r.Id != roleName))
            {
                // Обновление сведений пользователя
                roles.Add(new ForeignKey { Id = roleName, DisplayName = roleName });
                user.Roles = roles;
                await UpdateUserAsync(user);
            }
        }

        public async Task RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Resources.CantRemoveUnsavedUserFromRole);
            }

            var roles = user.Roles.ToList();

            if (roles.Any(r => r.Id == roleName))
            {
                // Обновление сведений пользователя
                user.Roles = roles.Where(r => r.Id != roleName).ToList();
                await UpdateUserAsync(user);
            }
        }

        public async Task AddUserClaimAsync(ApplicationUser user, string claimType, string claimValue)
        {
            if (!user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                var claims = user.Claims.ToList();
                claims.Add(new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue });
                user.Claims = claims;
                await UpdateUserAsync(user);
            }
        }

        public async Task RemoveUserClaimAsync(ApplicationUser user, string claimType, string claimValue)
        {
            if (user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                user.Claims = user.Claims.Where(c => !(c.Type.DisplayName == claimType && c.Value == claimValue)).ToList();
                await UpdateUserAsync(user);
            }
        }

        public async Task AddUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            var logins = user.Logins.ToList();

            if (!logins.Any(f => f.Provider == userLogin.ProviderKey && f.ProviderKey == userLogin.ProviderKey))
            {
                logins.Add(userLogin);
                user.Logins = logins;
                await UpdateUserAsync(user);
            }
        }

        public async Task RemoveUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            if (user.Logins.Any(f => f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey))
            {
                user.Logins = user.Logins.Where(f => !(f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey)).ToList();
                await UpdateUserAsync(user);
            }
        }

        private async Task<ApplicationUser> FindUser(Expression<Func<ApplicationUser, bool>> expression)
        {
            return await _userStorage.Value.Find(expression).FirstOrDefaultAsync();
        }

        private async Task SaveUser(ApplicationUser user)
        {
            user.SecurityStamp = CreateUnique();

            await _userStorage.Value.ReplaceOneAsync(user, f => f._id == user._id, true);
        }

        /// <summary>
        /// Обновляет сведения о пользователе в локальном кэше.
        /// </summary>
        private void UpdateUserInCache(ApplicationUser user)
        {
            _userCache.Value.AddOrUpdateUser(user);
        }

        /// <summary>
        /// Удаляет сведения о пользователе из локального кэша.
        /// </summary>
        private void RemoveUserFromCache(string userId)
        {
            _userCache.Value.RemoveUser(userId);
        }

        /// <summary>
        /// Ищет сведения о пользователе в локальном кэше.
        /// </summary>
        private async Task<ApplicationUser> FindUserInCacheAsync(Func<AppUserStoreCache, ApplicationUser> cacheSelector, Func<Task<ApplicationUser>> storageSelector)
        {
            var user = cacheSelector(_userCache.Value);

            if (user == null)
            {
                user = await storageSelector();

                if (user != null)
                {
                    _userCache.Value.AddOrUpdateUser(user);
                }
            }

            return user;
        }

        private static string CreateUnique()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
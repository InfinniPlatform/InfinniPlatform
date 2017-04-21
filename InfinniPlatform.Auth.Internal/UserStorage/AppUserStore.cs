using System;
using System.Linq;
using System.Linq.Expressions;

using InfinniPlatform.Auth.Identity.MongoDb;
using InfinniPlatform.Auth.Properties;
using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Auth.UserStorage
{
    internal class AppUserStore : IAppUserStore
    {
        public AppUserStore(Lazy<AppUserStoreCache> userCache, ISystemDocumentStorageFactory documentStorageFactory)
        {
            // Lazy, чтобы подписка на изменения кэша пользователей в кластере не создавалась сразу

            _userCache = userCache;
            _userStorage = new Lazy<ISystemDocumentStorage<IdentityUser>>(() => documentStorageFactory.GetStorage<IdentityUser>());
        }


        private readonly Lazy<AppUserStoreCache> _userCache;
        private readonly Lazy<ISystemDocumentStorage<IdentityUser>> _userStorage;


        public void CreateUser(IdentityUser user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = CreateUnique();
            }

            UpdateUser(user);
        }

        public void UpdateUser(IdentityUser user)
        {
            SaveUser(user);
            UpdateUserInCache(user);
        }

        public void DeleteUser(IdentityUser user)
        {
            _userStorage.Value.DeleteOne(f => f._id == user._id);

            RemoveUserFromCache(user.Id);
        }

        public IdentityUser FindUserById(string userId)
        {
            return FindUserInCache(c => c.FindUserById(userId), () => FindUser(f => f._header._deleted == null && f.Id == userId));
        }

        public IdentityUser FindUserByUserName(string userName)
        {
            return FindUserInCache(c => c.FindUserByUserName(userName), () => FindUser(f => f._header._deleted == null && f.UserName == userName));
        }

        public IdentityUser FindUserByEmail(string email)
        {
            return FindUserInCache(c => c.FindUserByEmail(email), () => FindUser(f => f._header._deleted == null && f.Email == email));
        }

        public IdentityUser FindUserByPhoneNumber(string phoneNumber)
        {
            return FindUserInCache(c => c.FindUserByPhoneNumber(phoneNumber), () => FindUser(f => f._header._deleted == null && f.PhoneNumber == phoneNumber));
        }

        public IdentityUser FindUserByLogin(IdentityUserLogin userLogin)
        {
            return FindUserInCache(c => c.FindUserByLogin(userLogin), () => FindUser(f => f._header._deleted == null && f.Logins.Any(l => l.ProviderKey == userLogin.ProviderKey)));
        }

        public IdentityUser FindUserByName(string name)
        {
            return FindUserByUserName(name) ?? FindUserByEmail(name) ?? FindUserByPhoneNumber(name);
        }

        public void AddUserToRole(IdentityUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Resources.CantAddUnsavedUserToRole);
            }

            var roles = user.Roles.ToList();

            if (roles.All(r => r != roleName))
            {
                // Обновление сведений пользователя
                roles.Add(roleName);
                user.Roles = roles;
                UpdateUser(user);
            }
        }

        public void RemoveUserFromRole(IdentityUser user, string roleName)
        {
            if (user.Id == null)
            {
                throw new ArgumentException(Resources.CantRemoveUnsavedUserFromRole);
            }

            var roles = user.Roles.ToList();

            if (roles.Any(r => r == roleName))
            {
                // Обновление сведений пользователя
                user.Roles = roles.Where(r => r != roleName).ToList();
                UpdateUser(user);
            }
        }

        public void AddUserClaim(IdentityUser user, string claimType, string claimValue)
        {
            if (!user.Claims.Any(c => c.Type == claimType && c.Value == claimValue))
            {
                var claims = user.Claims.ToList();
                claims.Add(new IdentityUserClaim { Type = claimType, Value = claimValue });
                user.Claims = claims;
                UpdateUser(user);
            }
        }

        public void RemoveUserClaim(IdentityUser user, string claimType, string claimValue)
        {
            if (user.Claims.Any(c => c.Type == claimType && c.Value == claimValue))
            {
                user.Claims = user.Claims.Where(c => !(c.Type == claimType && c.Value == claimValue)).ToList();
                UpdateUser(user);
            }
        }

        public void AddUserLogin(IdentityUser user, IdentityUserLogin userLogin)
        {
            var logins = user.Logins.ToList();

            if (!logins.Any(f => f.LoginProvider == userLogin.LoginProvider && f.ProviderKey == userLogin.ProviderKey))
            {
                logins.Add(userLogin);
                user.Logins = logins;
                UpdateUser(user);
            }
        }

        public void RemoveUserLogin(IdentityUser user, IdentityUserLogin userLogin)
        {
            if (user.Logins.Any(f => f.LoginProvider == userLogin.LoginProvider && f.ProviderKey == userLogin.ProviderKey))
            {
                user.Logins = user.Logins.Where(f => !(f.LoginProvider == userLogin.LoginProvider && f.ProviderKey == userLogin.ProviderKey)).ToList();
                UpdateUser(user);
            }
        }

        private IdentityUser FindUser(Expression<Func<IdentityUser, bool>> expression)
        {
            return _userStorage.Value.Find(expression).FirstOrDefault();
        }

        private void SaveUser(IdentityUser user)
        {
            user.SecurityStamp = CreateUnique();

            _userStorage.Value.ReplaceOne(user, f => f._id == user._id, true);
        }

        /// <summary>
        /// Обновляет сведения о пользователе в локальном кэше.
        /// </summary>
        private void UpdateUserInCache(IdentityUser user)
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
        private IdentityUser FindUserInCache(Func<AppUserStoreCache, IdentityUser> cacheSelector, Func<IdentityUser> storageSelector)
        {
            var user = cacheSelector(_userCache.Value);

            if (user == null)
            {
                user = storageSelector();

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
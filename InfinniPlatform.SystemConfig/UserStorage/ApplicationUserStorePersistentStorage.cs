using System;
using System.Linq;

using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.UserStorage
{
    internal sealed class ApplicationUserStorePersistentStorage : IApplicationUserStore
    {
        public ApplicationUserStorePersistentStorage(Lazy<ApplicationUserStoreCache> userCache, ElasticSearchUserStorage userStorage)
        {
            // Lazy, чтобы подписка на изменения кэша пользователей в кластере не создавалась сразу

            _userCache = userCache;
            _userStorage = userStorage;
        }


        private readonly Lazy<ApplicationUserStoreCache> _userCache;
        private readonly ElasticSearchUserStorage _userStorage;

        public void CreateUser(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = CreateUnique();
            }

            UpdateUser(user);
        }

        public void UpdateUser(ApplicationUser user)
        {
            SaveUser(user);
            UpdateUserInCache(user);
        }

        private void SaveUser(ApplicationUser user)
        {
            user.SecurityStamp = CreateUnique();

            _userStorage.Save(user.Id, JsonObjectSerializer.Default.ConvertToDynamic(user));
        }

        public void DeleteUser(ApplicationUser user)
        {
            _userStorage.Delete(user.Id);

            RemoveUserFromCache(user.Id);
        }

        public ApplicationUser FindUserById(string userId)
        {
            return FindUserInCache(c => c.FindUserById(userId), () => FindUser("Id", userId));
        }

        public ApplicationUser FindUserByUserName(string userName)
        {
            return FindUserInCache(c => c.FindUserByUserName(userName), () => FindUser("UserName", userName));
        }

        public ApplicationUser FindUserByEmail(string email)
        {
            return FindUserInCache(c => c.FindUserByEmail(email), () => FindUser("Email", email));
        }

        public ApplicationUser FindUserByPhoneNumber(string phoneNumber)
        {
            return FindUserInCache(c => c.FindUserByPhoneNumber(phoneNumber), () => FindUser("PhoneNumber", phoneNumber));
        }

        public ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin)
        {
            return FindUserInCache(c => c.FindUserByLogin(userLogin), () => FindUser("Logins.ProviderKey", userLogin.ProviderKey));
        }

        public ApplicationUser FindUserByName(string name)
        {
            return FindUserByUserName(name) ?? FindUserByEmail(name) ?? FindUserByPhoneNumber(name);
        }

        private ApplicationUser FindUser(string property, string value)
        {
            var user = _userStorage.Find(property, value);

            return (user != null)
                       ? JsonObjectSerializer.Default.ConvertFromDynamic<ApplicationUser>(user)
                       : null;
        }

        public void AddUserToRole(ApplicationUser user, string roleName)
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
                UpdateUser(user);

                // Добавление связки пользователь-роль
                dynamic userRole = new DynamicWrapper();
                userRole.UserName = user.UserName;
                userRole.RoleName = roleName;
            }
        }

        public void RemoveUserFromRole(ApplicationUser user, string roleName)
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
                UpdateUser(user);
            }
        }

        public void AddUserClaim(ApplicationUser user, string claimType, string claimValue)
        {
            if (!user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                var claims = user.Claims.ToList();
                claims.Add(new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue });
                user.Claims = claims;
                UpdateUser(user);
            }
        }

        public void RemoveUserClaim(ApplicationUser user, string claimType, string claimValue)
        {
            if (user.Claims.Any(c => c.Type.DisplayName == claimType && c.Value == claimValue))
            {
                user.Claims = user.Claims.Where(c => !(c.Type.DisplayName == claimType && c.Value == claimValue)).ToList();
                UpdateUser(user);
            }
        }

        public void AddUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            var logins = user.Logins.ToList();

            if (!logins.Any(f => f.Provider == userLogin.ProviderKey && f.ProviderKey == userLogin.ProviderKey))
            {
                logins.Add(userLogin);
                user.Logins = logins;
                UpdateUser(user);
            }
        }

        public void RemoveUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            if (user.Logins.Any(f => f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey))
            {
                user.Logins = user.Logins.Where(f => !(f.Provider == userLogin.Provider && f.ProviderKey == userLogin.ProviderKey)).ToList();
                UpdateUser(user);
            }
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
        private ApplicationUser FindUserInCache(Func<ApplicationUserStoreCache, ApplicationUser> cacheSelector, Func<ApplicationUser> storageSelector)
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
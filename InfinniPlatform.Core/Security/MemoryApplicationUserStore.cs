using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Security;

namespace InfinniPlatform.Security
{
    /// <summary>
    ///     Виртуальное хранилище сведений о пользователях системы.
    /// </summary>
    /// <remarks>
    ///     Данная реализация может быть использована в тестах или как руководство к созданию реального хранилища.
    /// </remarks>
    public sealed class MemoryApplicationUserStore : IApplicationUserStore
    {
        private readonly List<ApplicationUser> _users
            = new List<ApplicationUser>();

        public void CreateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Имя пользователя не задано
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new ArgumentException(@"User name cannot be null or witespace.");
            }

            // Имя пользователя уже занято
            if (FindUserByName(user.UserName) != null)
            {
                throw new ArgumentException(string.Format(@"User name '{0}' is already taken.", user.UserName));
            }

            // Обновление записи хранилища пользователей

            var userEntry = new ApplicationUser();
            UpdateEntry(userEntry, user);

            userEntry.Id = CreateUnique();
            userEntry.SecurityStamp = CreateUnique();

            _users.Add(userEntry);

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void UpdateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userEntry = FindUserById(user.Id);

            if (userEntry != null)
            {
                // Имя пользователя не задано
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    throw new ArgumentException(@"User name cannot be null or witespace.");
                }

                var userByName = FindUserByName(user.UserName);

                // Имя пользователя уже занято
                if (userByName != null && !StringEquals(userByName.Id, user.Id))
                {
                    throw new ArgumentException(string.Format(@"User name '{0}' is already taken.", user.UserName));
                }

                // Обновление записи хранилища пользователей
                UpdateEntry(userEntry, user);

                // Обновление сведений пользователя
                UpdateInfo(user, userEntry);
            }
            else
            {
                CreateUser(user);
            }
        }

        public void DeleteUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userById = FindUserById(user.Id);

            if (userById != null)
            {
                _users.Remove(userById);
            }
        }

        public ApplicationUser FindUserById(string userId)
        {
            return _users.FirstOrDefault(u => StringEquals(u.Id, userId));
        }

        public ApplicationUser FindUserByName(string userName)
        {
            return _users.FirstOrDefault(u => StringEquals(u.UserName, userName));
        }

        public ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            var loginProvider = userLogin.Provider;
            var providerKey = userLogin.ProviderKey;

            return
                _users.FirstOrDefault(
                    u =>
                        u.Logins != null &&
                        u.Logins.Any(
                            l => StringEquals(l.Provider, loginProvider) && StringEquals(l.ProviderKey, providerKey)));
        }

        public void AddUserToRole(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Поиск указанной роли пользователя
            var roleEntry = FindRoleByName(roleName);

            if (roleEntry == null)
            {
                throw new ArgumentException(string.Format("Role '{0}' does not exist.", roleName));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Roles == null)
            {
                userEntry.Roles = new List<ForeignKey>();
            }

            if (!userEntry.Roles.Any(r => StringEquals(r.Id, roleEntry.Id)))
            {
                userEntry.Roles = userEntry.Roles.Concat(new[] {roleEntry}).ToList();
            }

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void RemoveUserFromRole(ApplicationUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Поиск указанной роли пользователя
            var roleEntry = FindRoleByName(roleName);

            if (roleEntry == null)
            {
                throw new ArgumentException(string.Format("Role '{0}' does not exist.", roleName));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Roles == null)
            {
                userEntry.Roles = new List<ForeignKey>();
            }

            userEntry.Roles = userEntry.Roles.Where(r => !StringEquals(r.Id, roleEntry.Id)).ToList();

            if (userEntry.DefaultRole != null && StringEquals(userEntry.DefaultRole.Id, roleEntry.Id))
            {
                userEntry.DefaultRole = null;
            }

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void AddUserClaim(ApplicationUser user, string claimType, string claimValue)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException("claimType");
            }

            if (string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException("claimValue");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Поиск указанного типа утверждения
            var claimTypeEntry = FindClaimTypeByName(claimType);

            if (claimTypeEntry == null)
            {
                throw new ArgumentException(string.Format("Claim type '{0}' does not exist.", claimType));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Claims == null)
            {
                userEntry.Claims = new List<ApplicationUserClaim>();
            }

            userEntry.Claims =
                userEntry.Claims.Concat(new[] {new ApplicationUserClaim {Type = claimTypeEntry, Value = claimValue}});

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void RemoveUserClaim(ApplicationUser user, string claimType)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException("claimType");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Claims == null)
            {
                userEntry.Claims = new List<ApplicationUserClaim>();
            }

            userEntry.Claims =
                userEntry.Claims.Where(c => c.Type != null && !StringEquals(c.Type.Id, claimType)).ToList();

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void AddUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            if (string.IsNullOrWhiteSpace(userLogin.Provider))
            {
                throw new ArgumentException(@"Login provider cannot be null or witespace.");
            }

            if (string.IsNullOrWhiteSpace(userLogin.ProviderKey))
            {
                throw new ArgumentException(@"Provider key cannot be null or witespace.");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Logins == null)
            {
                userEntry.Logins = new List<ApplicationUserLogin>();
            }

            userEntry.Logins = userEntry.Logins.Concat(new[] {userLogin});

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void RemoveUserLogin(ApplicationUser user, ApplicationUserLogin userLogin)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            if (string.IsNullOrWhiteSpace(userLogin.Provider))
            {
                throw new ArgumentException(@"Login provider cannot be null or witespace.");
            }

            if (string.IsNullOrWhiteSpace(userLogin.ProviderKey))
            {
                throw new ArgumentException(@"Provider key cannot be null or witespace.");
            }

            // Поиск указанного пользователя
            var userEntry = FindUserById(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException(string.Format("User '{0}' does not exist.", user.Id));
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Logins == null)
            {
                userEntry.Logins = new List<ApplicationUserLogin>();
            }

            userEntry.Logins =
                userEntry.Logins.Where(
                    l =>
                        !StringEquals(l.Provider, userLogin.Provider) ||
                        !StringEquals(l.ProviderKey, userLogin.ProviderKey)).ToList();

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public void Dispose()
        {
        }

        // Helpers

        private static bool StringEquals(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }

        private static ForeignKey FindRoleByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            // Естественно, это фиктивный код

            return new ForeignKey {Id = roleName, DisplayName = roleName};
        }

        private static ForeignKey FindClaimTypeByName(string claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException("claimType");
            }

            // Естественно, это фиктивный код

            return new ForeignKey {Id = claimType, DisplayName = claimType};
        }

        private static void UpdateEntry(ApplicationUser userEntry, ApplicationUser userInfo)
        {
            userEntry.UserName = userInfo.UserName;
            userEntry.DisplayName = userInfo.DisplayName;
            userEntry.Description = userInfo.Description;
            userEntry.PasswordHash = userInfo.PasswordHash;
            userEntry.SecurityStamp = userInfo.SecurityStamp;

            if (userInfo.DefaultRole != null &&
                (userEntry.Roles == null || !userEntry.Roles.Any(r => StringEquals(r.Id, userInfo.DefaultRole.Id))))
            {
                throw new ArgumentException(string.Format(@"User '{0}' is not in role '{1}'.", userInfo.UserName,
                    userInfo.DefaultRole.Id));
            }

            userEntry.DefaultRole = userInfo.DefaultRole;
        }

        private static void UpdateInfo(ApplicationUser userInfo, ApplicationUser userEntry)
        {
            userInfo.Id = userEntry.Id;
            userInfo.UserName = userEntry.UserName;
            userInfo.DisplayName = userEntry.DisplayName;
            userInfo.Description = userEntry.Description;
            userInfo.PasswordHash = userEntry.PasswordHash;
            userInfo.SecurityStamp = userEntry.SecurityStamp;
            userInfo.DefaultRole = userEntry.DefaultRole;
            userInfo.Roles = userEntry.Roles;
            userInfo.Claims = userEntry.Claims;
            userInfo.Logins = userEntry.Logins;
        }

        private static string CreateUnique()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
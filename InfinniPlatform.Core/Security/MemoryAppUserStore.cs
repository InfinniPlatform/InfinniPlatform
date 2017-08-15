using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Core.Security
{
    /// <summary>
    /// Виртуальное хранилище сведений о пользователях системы.
    /// </summary>
    /// <remarks>
    /// Данная реализация может быть использована в тестах или как руководство к созданию реального хранилища.
    /// </remarks>
    public sealed class MemoryAppUserStore : IAppUserStore
    {
        private readonly List<ApplicationUser> _users = new List<ApplicationUser>();

        public async Task CreateUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Имя пользователя не задано
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                throw new ArgumentException(@"User name cannot be null or whitespace.");
            }

            // Имя пользователя уже занято
            if (await FindUserByNameAsync(user.UserName) != null)
            {
                throw new ArgumentException($@"User name '{user.UserName}' is already taken.");
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

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry != null)
            {
                // Имя пользователя не задано
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    throw new ArgumentException(@"User name cannot be null or witespace.");
                }

                var userByName = await FindUserByNameAsync(user.UserName);

                // Имя пользователя уже занято
                if (userByName != null && !StringEquals(userByName.Id, user.Id))
                {
                    throw new ArgumentException($@"User name '{user.UserName}' is already taken.");
                }

                // Обновление записи хранилища пользователей
                UpdateEntry(userEntry, user);

                // Обновление сведений пользователя
                UpdateInfo(user, userEntry);
            }
            else
            {
                await CreateUserAsync(user);
            }
        }

        public async Task DeleteUserAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userById = await FindUserByIdAsync(user.Id);

            if (userById != null)
            {
                _users.Remove(userById);
            }
        }

        public Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return Task.FromResult(_users.FirstOrDefault(u => StringEquals(u.Id, userId)));
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string name)
        {
            return await FindUserByUserNameAsync(name)
                   ?? await FindUserByEmailAsync(name)
                   ?? await FindUserByPhoneNumberAsync(name);
        }

        public Task<ApplicationUser> FindUserByUserNameAsync(string userName)
        {
            return Task.FromResult(_users.FirstOrDefault(u => StringEquals(u.UserName, userName)));
        }

        public Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => StringEquals(u.Email, email)));
        }

        public Task<ApplicationUser> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            return Task.FromResult(_users.FirstOrDefault(u => StringEquals(u.PhoneNumber, phoneNumber)));
        }

        public Task<ApplicationUser> FindUserByLoginAsync(ApplicationUserLogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException("userLogin");
            }

            var loginProvider = userLogin.Provider;
            var providerKey = userLogin.ProviderKey;

            var user = _users.FirstOrDefault(u => u.Logins != null && u.Logins.Any(l => StringEquals(l.Provider, loginProvider) && StringEquals(l.ProviderKey, providerKey)));
            return Task.FromResult(user);
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
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
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Поиск указанной роли пользователя
            var roleEntry = FindRoleByName(roleName);

            if (roleEntry == null)
            {
                throw new ArgumentException($"Role '{roleName}' does not exist.");
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Roles == null)
            {
                userEntry.Roles = new List<ForeignKey>();
            }

            if (!userEntry.Roles.Any(r => StringEquals(r.Id, roleEntry.Id)))
            {
                userEntry.Roles = userEntry.Roles.Concat(new[] { roleEntry }).ToList();
            }

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public async Task RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
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
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Поиск указанной роли пользователя
            var roleEntry = FindRoleByName(roleName);

            if (roleEntry == null)
            {
                throw new ArgumentException($"Role '{roleName}' does not exist.");
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

        public async Task AddUserClaimAsync(ApplicationUser user, string claimType, string claimValue)
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
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Поиск указанного типа утверждения
            var claimTypeEntry = FindClaimTypeByName(claimType);

            if (claimTypeEntry == null)
            {
                throw new ArgumentException($"Claim type '{claimType}' does not exist.");
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Claims == null)
            {
                userEntry.Claims = new List<ApplicationUserClaim>();
            }

            userEntry.Claims = userEntry.Claims.Concat(new[] { new ApplicationUserClaim { Type = claimTypeEntry, Value = claimValue } });

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public async Task RemoveUserClaimAsync(ApplicationUser user, string claimType, string claimValue)
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
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Claims == null)
            {
                userEntry.Claims = new List<ApplicationUserClaim>();
            }

            userEntry.Claims = userEntry.Claims.Where(c => c.Type != null && !StringEquals(c.Type.Id, claimType)).ToList();

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public async Task AddUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin)
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
                throw new ArgumentException(@"Login provider cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(userLogin.ProviderKey))
            {
                throw new ArgumentException(@"Provider key cannot be null or whitespace.");
            }

            // Поиск указанного пользователя
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Logins == null)
            {
                userEntry.Logins = new List<ApplicationUserLogin>();
            }

            userEntry.Logins = userEntry.Logins.Concat(new[] { userLogin });

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
        }

        public async Task RemoveUserLoginAsync(ApplicationUser user, ApplicationUserLogin userLogin)
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
                throw new ArgumentException(@"Login provider cannot be null or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(userLogin.ProviderKey))
            {
                throw new ArgumentException(@"Provider key cannot be null or whitespace.");
            }

            // Поиск указанного пользователя
            var userEntry = await FindUserByIdAsync(user.Id);

            if (userEntry == null)
            {
                throw new ArgumentException($"User '{user.Id}' does not exist.");
            }

            // Обновление записи хранилища пользователей

            if (userEntry.Logins == null)
            {
                userEntry.Logins = new List<ApplicationUserLogin>();
            }

            userEntry.Logins = userEntry.Logins.Where(l => !StringEquals(l.Provider, userLogin.Provider) || !StringEquals(l.ProviderKey, userLogin.ProviderKey)).ToList();

            // Обновление сведений пользователя
            UpdateInfo(user, userEntry);
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

            return new ForeignKey { Id = roleName, DisplayName = roleName };
        }

        private static ForeignKey FindClaimTypeByName(string claimType)
        {
            if (string.IsNullOrEmpty(claimType))
            {
                throw new ArgumentNullException("claimType");
            }

            // Естественно, это фиктивный код

            return new ForeignKey { Id = claimType, DisplayName = claimType };
        }

        private static void UpdateEntry(ApplicationUser userEntry, ApplicationUser userInfo)
        {
            userEntry.UserName = userInfo.UserName;
            userEntry.DisplayName = userInfo.DisplayName;
            userEntry.Description = userInfo.Description;
            userEntry.PasswordHash = userInfo.PasswordHash;
            userEntry.SecurityStamp = userInfo.SecurityStamp;

            if (userInfo.DefaultRole != null && (userEntry.Roles == null || !userEntry.Roles.Any(r => StringEquals(r.Id, userInfo.DefaultRole.Id))))
            {
                throw new ArgumentException($@"User '{userInfo.UserName}' is not in role '{userInfo.DefaultRole.Id}'.");
            }

            userEntry.DefaultRole = userInfo.DefaultRole;
            userEntry.Roles = userInfo.Roles;
            userEntry.Claims = userInfo.Claims;
            userEntry.Logins = userInfo.Logins;
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
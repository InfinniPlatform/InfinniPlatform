using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    /// <summary>
    /// Хранилище сведений о пользователях приложения <see cref="IdentityApplicationUser" />.
    /// </summary>
    /// <remarks>
    /// Данный класс представляет реализует множество интерфейсов из-за особенностей реализации
    /// <see cref="UserManager{TUser}" />.
    /// </remarks>
    internal sealed class IdentityApplicationUserStore : IUserStore<IdentityApplicationUser>,
                                                         IUserPasswordStore<IdentityApplicationUser>,
                                                         IUserSecurityStampStore<IdentityApplicationUser>,
                                                         IUserRoleStore<IdentityApplicationUser>,
                                                         IUserClaimStore<IdentityApplicationUser>,
                                                         IUserLoginStore<IdentityApplicationUser>,
                                                         IUserEmailStore<IdentityApplicationUser>,
                                                         IUserPhoneNumberStore<IdentityApplicationUser>
    {
        // Helpers

        static IdentityApplicationUserStore()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ApplicationUser, IdentityApplicationUser>());
        }

        public IdentityApplicationUserStore(IAppUserStore userStore)
        {
            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore));
            }

            _userStore = userStore;
        }

        private readonly IAppUserStore _userStore;

        // IUserClaimStore

        public Task<IList<Claim>> GetClaimsAsync(IdentityApplicationUser user)
        {
            var result = (user.Claims != null) ? (IList<Claim>)user.Claims.Where(c => c.Type != null && c.Value != null).Select(c => new Claim(c.Type.Id, c.Value)).ToList() : new List<Claim>();
            return Task.FromResult(result);
        }

        public async Task AddClaimAsync(IdentityApplicationUser user, Claim claim)
        {
            await InvokeUserStore(async (s, u, v) => await s.AddUserClaimAsync(u, v.Type, v.Value), user, claim);
        }

        public async Task RemoveClaimAsync(IdentityApplicationUser user, Claim claim)
        {
            await InvokeUserStore(async (s, u, v) => await s.RemoveUserClaimAsync(u, v.Type, v.Value), user, claim);
        }

        // IUserEmailStore

        public async Task SetEmailAsync(IdentityApplicationUser user, string email)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.Email = v;
                                      return Task.FromResult(0);
                                  }, user, email);
        }

        public Task<string> GetEmailAsync(IdentityApplicationUser user)
        {
            var result = user.Email;
            return Task.FromResult(result);
        }

        public async Task SetEmailConfirmedAsync(IdentityApplicationUser user, bool confirmed)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.EmailConfirmed = v;
                                      return Task.FromResult(0);
                                  }, user, confirmed);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityApplicationUser user)
        {
            var result = user.EmailConfirmed;
            return Task.FromResult(result);
        }

        public async Task<IdentityApplicationUser> FindByEmailAsync(string email)
        {
            return await InvokeUserStore(async (s, v) => ToIdentityUser(await s.FindUserByEmailAsync(v)), email);
        }

        // IUserLoginStore

        public async Task<IdentityApplicationUser> FindAsync(UserLoginInfo login)
        {
            return await InvokeUserStore(async (s, l) => ToIdentityUser(await s.FindUserByLoginAsync(ToApplicationUserLogin(l))), login);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityApplicationUser user)
        {
            var result = (user.Logins != null) ? (IList<UserLoginInfo>)user.Logins.Select(FromApplicationUserLogin).ToList() : new List<UserLoginInfo>();
            return Task.FromResult(result);
        }

        public async Task AddLoginAsync(IdentityApplicationUser user, UserLoginInfo login)
        {
            await InvokeUserStore(async (s, u, l) => await s.AddUserLoginAsync(u, ToApplicationUserLogin(l)), user, login);
        }

        public Task RemoveLoginAsync(IdentityApplicationUser user, UserLoginInfo login)
        {
            return InvokeUserStore(async (s, u, l) => await s.RemoveUserLoginAsync(u, ToApplicationUserLogin(l)), user, login);
        }

        // IUserPasswordStore

        public async Task SetPasswordHashAsync(IdentityApplicationUser user, string passwordHash)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.PasswordHash = v;
                                      return Task.FromResult(0);
                                  }, user, passwordHash);
        }

        public Task<string> GetPasswordHashAsync(IdentityApplicationUser user)
        {
            var result = user.PasswordHash;
            return Task.FromResult(result);
        }

        public Task<bool> HasPasswordAsync(IdentityApplicationUser user)
        {
            var result = !string.IsNullOrEmpty(user.PasswordHash);
            return Task.FromResult(result);
        }

        // IUserPhoneNumberStore

        public async Task SetPhoneNumberAsync(IdentityApplicationUser user, string phoneNumber)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.PhoneNumber = v;
                                      return Task.FromResult(0);
                                  }, user, phoneNumber);
        }

        public Task<string> GetPhoneNumberAsync(IdentityApplicationUser user)
        {
            var result = user.PhoneNumber;
            return Task.FromResult(result);
        }

        public async Task SetPhoneNumberConfirmedAsync(IdentityApplicationUser user, bool confirmed)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.PhoneNumberConfirmed = v;
                                      return Task.FromResult(0);
                                  }, user, confirmed);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityApplicationUser user)
        {
            var result = user.PhoneNumberConfirmed;
            return Task.FromResult(result);
        }

        // IUserRoleStore

        public async Task AddToRoleAsync(IdentityApplicationUser user, string roleName)
        {
            await InvokeUserStore(async (s, u, v) => await s.AddUserToRoleAsync(u, v), user, roleName);
        }

        public async Task RemoveFromRoleAsync(IdentityApplicationUser user, string roleName)
        {
            await InvokeUserStore(async (s, u, v) => await s.RemoveUserFromRoleAsync(u, v), user, roleName);
        }

        public Task<IList<string>> GetRolesAsync(IdentityApplicationUser user)
        {
            var result = (user.Roles != null) ? (IList<string>)user.Roles.Select(r => r.Id).ToList() : new List<string>();
            return Task.FromResult(result);
        }

        public Task<bool> IsInRoleAsync(IdentityApplicationUser user, string roleName)
        {
            var result = (user.Roles != null) && user.Roles.Any(r => string.Equals(r.Id, roleName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }

        // IUserSecurityStampStore

        public async Task SetSecurityStampAsync(IdentityApplicationUser user, string stamp)
        {
            await InvokeUserStore((s, u, v) =>
                                  {
                                      u.SecurityStamp = v;
                                      return Task.FromResult(0);
                                  }, user, stamp);
        }

        public Task<string> GetSecurityStampAsync(IdentityApplicationUser user)
        {
            var result = user.SecurityStamp;
            return Task.FromResult(result);
        }

        // IUserStore

        public async Task CreateAsync(IdentityApplicationUser user)
        {
            await InvokeUserStore(async (s, u) => await s.CreateUserAsync(u), user);
        }

        public async Task UpdateAsync(IdentityApplicationUser user)
        {
            await InvokeUserStore(async (s, u) => await s.UpdateUserAsync(u), user);
        }

        public async Task DeleteAsync(IdentityApplicationUser user)
        {
            await InvokeUserStore(async (s, u) => await s.DeleteUserAsync(u), user);
        }

        public async Task<IdentityApplicationUser> FindByIdAsync(string userId)
        {
            return await InvokeUserStore(async (s, v) => ToIdentityUser(await s.FindUserByIdAsync(v)), userId);
        }

        public async Task<IdentityApplicationUser> FindByNameAsync(string userName)
        {
            return await InvokeUserStore(async (s, v) => ToIdentityUser(await s.FindUserByNameAsync(v)), userName);
        }

        // IDisposable

        public void Dispose()
        {
        }

        public async Task<IdentityApplicationUser> FindByUserNameAsync(string userName)
        {
            return await InvokeUserStore(async (s, v) => ToIdentityUser(await s.FindUserByUserNameAsync(v)), userName);
        }

        private static ApplicationUserLogin ToApplicationUserLogin(UserLoginInfo login)
        {
            return new ApplicationUserLogin { Provider = login.LoginProvider, ProviderKey = login.ProviderKey };
        }

        private static UserLoginInfo FromApplicationUserLogin(ApplicationUserLogin login)
        {
            return new UserLoginInfo(login.Provider, login.ProviderKey);
        }

        public async Task<IdentityApplicationUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await InvokeUserStore(async (s, v) => ToIdentityUser(await s.FindUserByPhoneNumberAsync(v)), phoneNumber);
        }

        private static IdentityApplicationUser ToIdentityUser(ApplicationUser user)
        {
            return Mapper.Map<IdentityApplicationUser>(user);
        }

        private async Task InvokeUserStore<T1>(Func<IAppUserStore, T1, Task> action, T1 arg1)
        {
            await action(_userStore, arg1);
        }

        private async Task InvokeUserStore<T1, T2>(Func<IAppUserStore, T1, T2, Task> action, T1 arg1, T2 arg2)
        {
            await action(_userStore, arg1, arg2);
        }

        private async Task<T> InvokeUserStore<T, T1>(Func<IAppUserStore, T1, Task<T>> action, T1 arg1)
        {
            return await action(_userStore, arg1);
        }
    }
}
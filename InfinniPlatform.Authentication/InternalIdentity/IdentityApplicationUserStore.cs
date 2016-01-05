using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using InfinniPlatform.Core.Security;

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
            Mapper.CreateMap<ApplicationUser, IdentityApplicationUser>();
        }

        public IdentityApplicationUserStore(IApplicationUserStore userStore)
        {
            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore));
            }

            _userStore = userStore;
        }

        private readonly IApplicationUserStore _userStore;

        // IUserClaimStore

        public Task<IList<Claim>> GetClaimsAsync(IdentityApplicationUser user)
        {
            var result = (user.Claims != null) ? (IList<Claim>)user.Claims.Where(c => c.Type != null && c.Value != null).Select(c => new Claim(c.Type.Id, c.Value)).ToList() : new List<Claim>();
            return Task.FromResult(result);
        }

        public Task AddClaimAsync(IdentityApplicationUser user, Claim claim)
        {
            return InvokeUserStore((s, u, v) => s.AddUserClaim(u, v.Type, v.Value), user, claim);
        }

        public Task RemoveClaimAsync(IdentityApplicationUser user, Claim claim)
        {
            return InvokeUserStore((s, u, v) => s.RemoveUserClaim(u, v.Type, v.Value), user, claim);
        }

        // IUserEmailStore

        public Task SetEmailAsync(IdentityApplicationUser user, string email)
        {
            return InvokeUserStore((s, u, v) => { u.Email = v; }, user, email);
        }

        public Task<string> GetEmailAsync(IdentityApplicationUser user)
        {
            var result = user.Email;
            return Task.FromResult(result);
        }

        public Task SetEmailConfirmedAsync(IdentityApplicationUser user, bool confirmed)
        {
            return InvokeUserStore((s, u, v) => { u.EmailConfirmed = v; }, user, confirmed);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityApplicationUser user)
        {
            var result = user.EmailConfirmed;
            return Task.FromResult(result);
        }

        public Task<IdentityApplicationUser> FindByEmailAsync(string email)
        {
            return InvokeUserStore((s, v) => ToIdentityUser(s.FindUserByEmail(v)), email);
        }

        // IUserLoginStore

        public Task<IdentityApplicationUser> FindAsync(UserLoginInfo login)
        {
            return InvokeUserStore((s, l) => ToIdentityUser(s.FindUserByLogin(ToApplicationUserLogin(l))), login);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityApplicationUser user)
        {
            var result = (user.Logins != null) ? (IList<UserLoginInfo>)user.Logins.Select(FromApplicationUserLogin).ToList() : new List<UserLoginInfo>();
            return Task.FromResult(result);
        }

        public Task AddLoginAsync(IdentityApplicationUser user, UserLoginInfo login)
        {
            return InvokeUserStore((s, u, l) => s.AddUserLogin(u, ToApplicationUserLogin(l)), user, login);
        }

        public Task RemoveLoginAsync(IdentityApplicationUser user, UserLoginInfo login)
        {
            return InvokeUserStore((s, u, l) => s.RemoveUserLogin(u, ToApplicationUserLogin(l)), user, login);
        }

        // IUserPasswordStore

        public Task SetPasswordHashAsync(IdentityApplicationUser user, string passwordHash)
        {
            return InvokeUserStore((s, u, v) => { u.PasswordHash = v; }, user, passwordHash);
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

        public Task SetPhoneNumberAsync(IdentityApplicationUser user, string phoneNumber)
        {
            return InvokeUserStore((s, u, v) => { u.PhoneNumber = v; }, user, phoneNumber);
        }

        public Task<string> GetPhoneNumberAsync(IdentityApplicationUser user)
        {
            var result = user.PhoneNumber;
            return Task.FromResult(result);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityApplicationUser user, bool confirmed)
        {
            return InvokeUserStore((s, u, v) => { u.PhoneNumberConfirmed = v; }, user, confirmed);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityApplicationUser user)
        {
            var result = user.PhoneNumberConfirmed;
            return Task.FromResult(result);
        }

        // IUserRoleStore

        public Task AddToRoleAsync(IdentityApplicationUser user, string roleName)
        {
            return InvokeUserStore((s, u, v) => s.AddUserToRole(u, v), user, roleName);
        }

        public Task RemoveFromRoleAsync(IdentityApplicationUser user, string roleName)
        {
            return InvokeUserStore((s, u, v) => s.RemoveUserFromRole(u, v), user, roleName);
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

        public Task SetSecurityStampAsync(IdentityApplicationUser user, string stamp)
        {
            return InvokeUserStore((s, u, v) => { u.SecurityStamp = v; }, user, stamp);
        }

        public Task<string> GetSecurityStampAsync(IdentityApplicationUser user)
        {
            var result = user.SecurityStamp;
            return Task.FromResult(result);
        }

        // IUserStore

        public Task CreateAsync(IdentityApplicationUser user)
        {
            return InvokeUserStore((s, u) => s.CreateUser(u), user);
        }

        public Task UpdateAsync(IdentityApplicationUser user)
        {
            return InvokeUserStore((s, u) => s.UpdateUser(u), user);
        }

        public Task DeleteAsync(IdentityApplicationUser user)
        {
            return InvokeUserStore((s, u) => s.DeleteUser(u), user);
        }

        public Task<IdentityApplicationUser> FindByIdAsync(string userId)
        {
            return InvokeUserStore((s, v) => ToIdentityUser(s.FindUserById(v)), userId);
        }

        public Task<IdentityApplicationUser> FindByNameAsync(string userName)
        {
            return InvokeUserStore((s, v) => ToIdentityUser(s.FindUserByName(v)), userName);
        }

        // IDisposable

        public void Dispose()
        {
        }

        public Task<IdentityApplicationUser> FindByUserNameAsync(string userName)
        {
            return InvokeUserStore((s, v) => ToIdentityUser(s.FindUserByUserName(v)), userName);
        }

        private static ApplicationUserLogin ToApplicationUserLogin(UserLoginInfo login)
        {
            return new ApplicationUserLogin { Provider = login.LoginProvider, ProviderKey = login.ProviderKey };
        }

        private static UserLoginInfo FromApplicationUserLogin(ApplicationUserLogin login)
        {
            return new UserLoginInfo(login.Provider, login.ProviderKey);
        }

        public Task<IdentityApplicationUser> FindByPhoneNumberAsync(string phoneNumber)
        {
            return InvokeUserStore((s, v) => ToIdentityUser(s.FindUserByPhoneNumber(v)), phoneNumber);
        }

        private static IdentityApplicationUser ToIdentityUser(ApplicationUser user)
        {
            return Mapper.Map<IdentityApplicationUser>(user);
        }

        private Task InvokeUserStore<T1>(Action<IApplicationUserStore, T1> action, T1 arg1)
        {
            action(_userStore, arg1);

            return Task.FromResult<object>(null);
        }

        private Task InvokeUserStore<T1, T2>(Action<IApplicationUserStore, T1, T2> action, T1 arg1, T2 arg2)
        {
            action(_userStore, arg1, arg2);

            return Task.FromResult<object>(null);
        }

        private Task<T> InvokeUserStore<T, T1>(Func<IApplicationUserStore, T1, T> action, T1 arg1)
        {
            var result = action(_userStore, arg1);

            return Task.FromResult(result);
        }
    }
}
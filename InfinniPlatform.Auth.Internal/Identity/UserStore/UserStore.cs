using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.UserStore
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    /// <typeparam name="TUser">Пользователь.</typeparam>
    public class UserStore<TUser> : IUserStore<TUser>,
                                    IUserPasswordStore<TUser>,
                                    IUserRoleStore<TUser>,
                                    IUserLoginStore<TUser>,
                                    IUserSecurityStampStore<TUser>,
                                    IUserEmailStore<TUser>,
                                    IUserClaimStore<TUser>,
                                    IUserPhoneNumberStore<TUser>,
                                    IUserTwoFactorStore<TUser>,
                                    IUserLockoutStore<TUser>,
                                    IUserAuthenticationTokenStore<TUser>
        where TUser : IdentityUser
    {
        private readonly ISystemDocumentStorage<TUser> _users;
        private readonly UserCache.UserCache<IdentityUser> _userCache;

        public UserStore(ISystemDocumentStorage<TUser> users, UserCache.UserCache<IdentityUser> userCache)
        {
            _users = users;
            _userCache = userCache;
        }

        public virtual Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken token)
        {
            return Task.Run(()=> user.SetToken(loginProvider, name, value), token);
        }

        public virtual Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            return Task.Run(()=> user.RemoveToken(loginProvider, name), token);
        }

        public virtual Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken token)
        {
            return Task.Run(() => user.GetTokenValue(loginProvider, name), token);
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() => user.Claims.Select(c => c.ToSecurityClaim()) as IList<Claim>, token);
        }

        public virtual Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.AddClaim(claim);
                                }

                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.RemoveClaim(claim);
                                }

                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual  Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken token = default(CancellationToken))
        {
            return Task.Run(()=> user.ReplaceClaim(claim, newClaim), token);
        }

        public virtual async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken token = default(CancellationToken))
        {
            return await _users.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)).ToListAsync();
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            return Task.Run(()=> user.EmailConfirmed = confirmed, token);
        }

        public virtual Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            return Task.Run(() => user.Email = email, token);
        }

        public virtual Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Email);
        }

        public virtual Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public virtual Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken token)
        {
            return Task.Run(() => user.NormalizedEmail = normalizedEmail, token);
        }

        public virtual Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            return FindUserInCache(() => (TUser) _userCache.FindUserByEmail(normalizedEmail), () => _users.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefault());
        }

        public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                var lockoutEndDateUtc = user.LockoutEndDateUtc;
                                return lockoutEndDateUtc.HasValue ? lockoutEndDateUtc.GetValueOrDefault() : new DateTimeOffset?();
                            }, token);
        }

        public virtual Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            return Task.Run(()=> user.LockoutEndDateUtc = lockoutEnd.HasValue ? lockoutEnd.GetValueOrDefault().UtcDateTime : new DateTime?(), token);
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                ++user.AccessFailedCount;
                                return user.AccessFailedCount;
                            }, token);
        }

        public virtual Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.Run(()=> user.AccessFailedCount = 0, token);
        }

        public virtual Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            return Task.Run(() => user.LockoutEnabled = enabled, token);
        }

        public virtual Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                user.AddLogin(login);
                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() =>
                            {
                                user.RemoveLogin(loginProvider, providerKey);
                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() => user.Logins.Select(l => l.ToUserLoginInfo()) as IList<UserLoginInfo>, token);
        }

        public virtual Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken token = default(CancellationToken))
        {
            return FindUserInCache(() => (TUser) _userCache.FindUserByLogin(loginProvider, providerKey),
                                   () => _users.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).FirstOrDefault());
        }

        public virtual void Dispose()
        {
        }

        public virtual async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            await _users.InsertOneAsync(user);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            await _users.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await _users.DeleteOneAsync(u => u.Id == user.Id);
            RemoveUserFromCache(user.Id);
            return IdentityResult.Success;
        }

        public virtual Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Id);
        }

        public virtual Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            return Task.Run(() => user.UserName = userName, token);
        }

        public virtual Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken token)
        {
            return Task.Run(() => user.NormalizedUserName = normalizedUserName, token);
        }

        public virtual async Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser)_userCache.FindUserById(userId), () => _users.Find(u => u._id.Equals(userId)).FirstOrDefault());
        }

        public virtual async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) _userCache.FindUserByUserName(normalizedUserName),
                                         () => _users.Find(u => u.UserName == normalizedUserName.ToLower()).FirstOrDefault());
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            return Task.Run(() => user.PasswordHash = passwordHash, token);
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.HasPassword());
        }

        public virtual Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            return Task.Run(()=> user.PhoneNumber = phoneNumber, token);
        }

        public virtual Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            return Task.Run(()=> user.PhoneNumberConfirmed = confirmed, token);
        }

        public virtual Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                user.AddRole(normalizedRoleName);
                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                user.RemoveRole(normalizedRoleName);
                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Roles as IList<string>);
        }

        public virtual Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            return Task.FromResult(user.Roles.Contains(normalizedRoleName));
        }

        public virtual async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken token)
        {
            return await _users.Find(u => u.Roles.Contains(normalizedRoleName)).ToListAsync();
        }

        public virtual Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken token)
        {
            return Task.Run(()=> user.SecurityStamp = stamp, token);
        }

        public virtual Task<string> GetSecurityStampAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            return Task.Run(()=> user.TwoFactorEnabled = enabled, token);
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Обновляет сведения о пользователе в локальном кэше.
        /// </summary>
        private void UpdateUserInCache(IdentityUser user)
        {
            _userCache.AddOrUpdateUser(user);
        }

        /// <summary>
        /// Удаляет сведения о пользователе из локального кэша.
        /// </summary>
        private void RemoveUserFromCache(string userId)
        {
            _userCache.RemoveUser(userId);
        }

        /// <summary>
        /// Ищет сведения о пользователе в локальном кэше.
        /// </summary>
        private Task<TUser> FindUserInCache(Func<TUser> cacheSelector, Func<TUser> storageSelector)
        {
            return Task.Run(() =>
                            {
                                var user = cacheSelector();

                                if (user == null)
                                {
                                    user = storageSelector();

                                    if (user != null)
                                    {
                                        _userCache.AddOrUpdateUser(user);
                                    }
                                }

                                return user;
                            });
        }
    }
}
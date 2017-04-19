using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Abstractions;

using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.MongoDb
{
    public class UserStore<TUser> : IUserPasswordStore<TUser>,
                                    IUserRoleStore<TUser>,
                                    IUserLoginStore<TUser>,
                                    IUserSecurityStampStore<TUser>,
                                    IUserEmailStore<TUser>,
                                    IUserClaimStore<TUser>,
                                    IUserPhoneNumberStore<TUser>,
                                    IUserTwoFactorStore<TUser>,
                                    IUserLockoutStore<TUser>,
                                    //IQueryableUserStore<TUser>,
                                    IUserAuthenticationTokenStore<TUser>
        where TUser : IdentityUser
    {
        private readonly IDocumentStorage<TUser> _users;

        public UserStore(ISystemDocumentStorage<TUser> users)
        {
            _users = users;
        }

        //public virtual IQueryable<TUser> Users => _users.AsQueryable();

        public virtual async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            user.SetToken(loginProvider, name, value);
        }

        public virtual async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.RemoveToken(loginProvider, name);
        }

        public virtual async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return user.GetTokenValue(loginProvider, name);
        }

        public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            return user.Claims.Select(c => c.ToSecurityClaim()).ToList();
        }

        public virtual Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.AddClaim(claim);
            }
            return Task.FromResult(0);
        }

        public virtual Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.RemoveClaim(claim);
            }
            return Task.FromResult(0);
        }

        public virtual async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            user.ReplaceClaim(claim, newClaim);
        }

        public virtual async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _users.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)).ToListAsync();
        }

        public virtual async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken token)
        {
            return user.EmailConfirmed;
        }

        public virtual async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.EmailConfirmed = confirmed;
        }

        public virtual async Task SetEmailAsync(TUser user, string email, CancellationToken token)
        {
            user.Email = email;
        }

        public virtual async Task<string> GetEmailAsync(TUser user, CancellationToken token)
        {
            return user.Email;
        }

        public virtual async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public virtual async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
        }

        public virtual Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken token)
        {
            return _users.Find(u => u.NormalizedEmail == normalizedEmail).FirstOrDefaultAsync();
        }

        public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken token)
        {
            var lockoutEndDateUtc = user.LockoutEndDateUtc;
            return Task.FromResult(lockoutEndDateUtc.HasValue ? (DateTimeOffset) lockoutEndDateUtc.GetValueOrDefault() : new DateTimeOffset?());
        }

        public virtual Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken token)
        {
            user.LockoutEndDateUtc = lockoutEnd.HasValue ? lockoutEnd.GetValueOrDefault().UtcDateTime : new DateTime?();
            return Task.FromResult(0);
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            ++user.AccessFailedCount;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public virtual async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken token)
        {
            return user.AccessFailedCount;
        }

        public virtual async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken token)
        {
            return user.LockoutEnabled;
        }

        public virtual async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            user.LockoutEnabled = enabled;
        }

        public virtual async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken token)
        {
            user.AddLogin(login);
        }

        public virtual async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            user.RemoveLogin(loginProvider, providerKey);
        }

        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken token)
        {
            return user.Logins.Select(l => l.ToUserLoginInfo()).ToList();
        }

        public virtual Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _users.Find(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey)).FirstOrDefaultAsync();
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
            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await _users.DeleteOneAsync(u => u.Id == user.Id);
            return IdentityResult.Success;
        }

        public virtual async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return user.Id;
        }

        public virtual async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return user.UserName;
        }

        public virtual async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
        }

        public virtual async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return user.NormalizedUserName;
        }

        public virtual async Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedUserName;
        }

        public virtual Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            return _users.Find(u => u._id.Equals(userId)).FirstOrDefaultAsync();
        }

        public virtual Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
        {
            return _users.Find(u => u.UserName == normalizedUserName.ToLower()).FirstOrDefaultAsync();
        }

        public virtual async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken token)
        {
            user.PasswordHash = passwordHash;
        }

        public virtual async Task<string> GetPasswordHashAsync(TUser user, CancellationToken token)
        {
            return user.PasswordHash;
        }

        public virtual async Task<bool> HasPasswordAsync(TUser user, CancellationToken token)
        {
            return user.HasPassword();
        }

        public virtual Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
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
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public virtual async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.AddRole(normalizedRoleName);
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            user.RemoveRole(normalizedRoleName);
        }

        public virtual async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token)
        {
            return user.Roles;
        }

        public virtual async Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken token)
        {
            return user.Roles.Contains(normalizedRoleName);
        }

        public virtual async Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken token)
        {
            return await _users.Find(u => u.Roles.Contains(normalizedRoleName)).ToListAsync();
        }

        public virtual async Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken token)
        {
            user.SecurityStamp = stamp;
        }

        public virtual async Task<string> GetSecurityStampAsync(TUser user, CancellationToken token)
        {
            return user.SecurityStamp;
        }

        public virtual Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken token)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
    }
}
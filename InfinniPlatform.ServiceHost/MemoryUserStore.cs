using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.ServiceHost
{
    public class MemoryUserStore<TUser> : IUserStore<TUser>,
                                          IUserClaimStore<TUser>,
                                          IUserEmailStore<TUser>,
                                          IUserPasswordStore<TUser>,
                                          IUserPhoneNumberStore<TUser>,
                                          IUserRoleStore<TUser>,
                                          IUserSecurityStampStore<TUser> where TUser : AppUser
    {
        private readonly ConcurrentDictionary<string, TUser> _users;

        public MemoryUserStore()
        {
            _users = new ConcurrentDictionary<string, TUser>();
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<Claim>) user.Claims.Select(c=>c.ToSecurityClaim()).ToList());
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.AddClaim(claim);
                                }
                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.ReplaceClaim(claim, newClaim);

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.RemoveClaim(claim);
                                }

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<TUser>() as IList<TUser>);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.Email = email;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.EmailConfirmed = confirmed;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return _users.Values.FirstOrDefault(u => u.NormalizedEmail == normalizedEmail); }, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.NormalizedEmail = normalizedEmail;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.PasswordHash = passwordHash;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.HasPassword(), cancellationToken);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.PhoneNumber = phoneNumber;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.PhoneNumberConfirmed = confirmed;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.AddRole(roleName);

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.RemoveRole(roleName);

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>) user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => user.Roles.Contains(roleName), cancellationToken);
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return _users.Values.Where(u => u.Roles.Contains(roleName)) as IList<TUser>; }, cancellationToken);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.SecurityStamp = stamp;

                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.UserName = userName;
                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                user.NormalizedUserName = normalizedName;
                                _users[user.UserName] = user;
                            }, cancellationToken);
        }

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                _users[user.UserName] = user;
                                return IdentityResult.Success;
                            }, cancellationToken);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                _users[user.UserName] = user;
                                return IdentityResult.Success;
                            }, cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
                            {
                                _users.TryRemove(user.UserName, out TUser u);
                                return IdentityResult.Success;
                            }, cancellationToken);
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return _users.Values.FirstOrDefault(u => u.Id == userId); }, cancellationToken);
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.Run(() => { return _users.Values.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName); }, cancellationToken);
        }
    }
}
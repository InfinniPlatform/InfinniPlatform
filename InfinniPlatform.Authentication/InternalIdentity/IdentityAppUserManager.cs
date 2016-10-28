using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Core.Threading;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    internal sealed class IdentityAppUserManager : IAppUserManager
    {
        public IdentityAppUserManager(IOwinContextProvider owinContextProvider, IUserIdentityProvider userIdentityProvider)
        {
            _owinContextProvider = owinContextProvider;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly IOwinContextProvider _owinContextProvider;
        private readonly IUserIdentityProvider _userIdentityProvider;


        private IOwinContext OwinContext => _owinContextProvider.GetOwinContext();

        private UserManager<IdentityApplicationUser> UserManager => OwinContext.GetUserManager<UserManager<IdentityApplicationUser>>();


        public ApplicationUser GetCurrentUser()
        {
            return InvokeUserManager((m, userId) => m.FindByIdAsync(userId));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.FindByIdAsync(userId));
        }

        // CRUD

        public ApplicationUser FindUserByName(string userName)
        {
            return InvokeUserManager((m, u) => Task.FromResult(u), userName);
        }

        public async Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => Task.FromResult(u), userName);
        }

        public void CreateUser(string userName, string password, string email = null)
        {
            var result = AsyncHelper.RunSync(() => UserManager.CreateAsync(new IdentityApplicationUser { UserName = userName, Email = email }, password));
            ThrowIfError(result);
        }

        public async Task CreateUserAsync(string userName, string password, string email = null)
        {
            var result = await UserManager.CreateAsync(new IdentityApplicationUser { UserName = userName, Email = email }, password);
            ThrowIfError(result);
        }

        public void DeleteUser(string userName)
        {
            InvokeUserManager((m, u) => m.DeleteAsync(u), userName);
        }

        public async Task DeleteUserAsync(string userName)
        {
            await InvokeUserManagerAsync((m, u) => m.DeleteAsync(u), userName);
        }

        // Password

        public bool HasPassword()
        {
            return InvokeUserManager((m, userId) => m.HasPasswordAsync(userId));
        }

        public async Task<bool> HasPasswordAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.HasPasswordAsync(userId));
        }

        public bool HasPassword(string userName)
        {
            return InvokeUserManager((m, u) => m.HasPasswordAsync(u.Id), userName);
        }

        public async Task<bool> HasPasswordAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.HasPasswordAsync(u.Id), userName);
        }

        public void AddPassword(string password)
        {
            InvokeUserManager((m, userId) => m.AddPasswordAsync(userId, password));
        }

        public async Task AddPasswordAsync(string password)
        {
            await InvokeUserManagerAsync((m, userId) => m.AddPasswordAsync(userId, password));
        }

        public void AddPassword(string userName, string password)
        {
            InvokeUserManager((m, u) => m.AddPasswordAsync(u.Id, password), userName);
        }

        public async Task AddPasswordAsync(string userName, string password)
        {
            await InvokeUserManagerAsync((m, u) => m.AddPasswordAsync(u.Id, password), userName);
        }

        public void RemovePassword()
        {
            InvokeUserManager((m, userId) => m.RemovePasswordAsync(userId));
        }

        public async Task RemovePasswordAsync()
        {
            await InvokeUserManagerAsync((m, userId) => m.RemovePasswordAsync(userId));
        }

        public void RemovePassword(string userName)
        {
            InvokeUserManager((m, u) => m.RemovePasswordAsync(u.Id), userName);
        }

        public async Task RemovePasswordAsync(string userName)
        {
            await InvokeUserManagerAsync((m, u) => m.RemovePasswordAsync(u.Id), userName);
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            InvokeUserManager((m, userId) => m.ChangePasswordAsync(userId, currentPassword, newPassword));
        }

        public async Task ChangePasswordAsync(string currentPassword, string newPassword)
        {
            await InvokeUserManagerAsync((m, userId) => m.ChangePasswordAsync(userId, currentPassword, newPassword));
        }

        public void ChangePassword(string userName, string currentPassword, string newPassword)
        {
            InvokeUserManager((m, u) => m.ChangePasswordAsync(u.Id, currentPassword, newPassword), userName);
        }

        public async Task ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            await InvokeUserManagerAsync((m, u) => m.ChangePasswordAsync(u.Id, currentPassword, newPassword), userName);
        }

        // SecurityStamp

        public string GetSecurityStamp()
        {
            return InvokeUserManager((m, userId) => m.GetSecurityStampAsync(userId));
        }

        public async Task<string> GetSecurityStampAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.GetSecurityStampAsync(userId));
        }

        public string GetSecurityStamp(string userName)
        {
            return InvokeUserManager((m, u) => m.GetSecurityStampAsync(u.Id), userName);
        }

        public async Task<string> GetSecurityStampAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.GetSecurityStampAsync(u.Id), userName);
        }

        public void UpdateSecurityStamp()
        {
            InvokeUserManager((m, userId) => m.UpdateSecurityStampAsync(userId));
        }

        public async Task UpdateSecurityStampAsync()
        {
            await InvokeUserManagerAsync((m, userId) => m.UpdateSecurityStampAsync(userId));
        }

        public void UpdateSecurityStamp(string userName)
        {
            InvokeUserManager((m, u) => m.UpdateSecurityStampAsync(u.Id), userName);
        }

        public async Task UpdateSecurityStampAsync(string userName)
        {
            await InvokeUserManagerAsync((m, u) => m.UpdateSecurityStampAsync(u.Id), userName);
        }

        // Email

        public string GetEmail()
        {
            return InvokeUserManager((m, userId) => m.GetEmailAsync(userId));
        }

        public async Task<string> GetEmailAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.GetEmailAsync(userId));
        }

        public string GetEmail(string userName)
        {
            return InvokeUserManager((m, u) => m.GetEmailAsync(u.Id), userName);
        }

        public async Task<string> GetEmailAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.GetEmailAsync(u.Id), userName);
        }

        public void SetEmail(string email)
        {
            InvokeUserManager((m, userId) => m.SetEmailAsync(userId, email));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.Email, email);
        }

        public async Task SetEmailAsync(string email)
        {
            await InvokeUserManagerAsync((m, userId) => m.SetEmailAsync(userId, email));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.Email, email);
        }

        public void SetEmail(string userName, string email)
        {
            InvokeUserManager((m, u) => m.SetEmailAsync(u.Id, email), userName);
        }

        public async Task SetEmailAsync(string userName, string email)
        {
            await InvokeUserManagerAsync((m, u) => m.SetEmailAsync(u.Id, email), userName);
        }

        public bool IsEmailConfirmed()
        {
            return InvokeUserManager((m, userId) => m.IsEmailConfirmedAsync(userId));
        }

        public async Task<bool> IsEmailConfirmedAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.IsEmailConfirmedAsync(userId));
        }

        public bool IsEmailConfirmed(string userName)
        {
            return InvokeUserManager((m, u) => m.IsEmailConfirmedAsync(u.Id), userName);
        }

        public async Task<bool> IsEmailConfirmedAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.IsEmailConfirmedAsync(u.Id), userName);
        }

        // PhoneNumber

        public string GetPhoneNumber()
        {
            return InvokeUserManager((m, userId) => m.GetPhoneNumberAsync(userId));
        }

        public async Task<string> GetPhoneNumberAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.GetPhoneNumberAsync(userId));
        }

        public string GetPhoneNumber(string userName)
        {
            return InvokeUserManager((m, u) => m.GetPhoneNumberAsync(u.Id), userName);
        }

        public async Task<string> GetPhoneNumberAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.GetPhoneNumberAsync(u.Id), userName);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            InvokeUserManager((m, userId) => m.SetPhoneNumberAsync(userId, phoneNumber));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.MobilePhone, phoneNumber);
        }

        public async Task SetPhoneNumberAsync(string phoneNumber)
        {
            await InvokeUserManagerAsync((m, userId) => m.SetPhoneNumberAsync(userId, phoneNumber));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.MobilePhone, phoneNumber);
        }

        public void SetPhoneNumber(string userName, string phoneNumber)
        {
            InvokeUserManager((m, u) => m.SetPhoneNumberAsync(u.Id, phoneNumber), userName);
        }

        public async Task SetPhoneNumberAsync(string userName, string phoneNumber)
        {
            await InvokeUserManagerAsync((m, u) => m.SetPhoneNumberAsync(u.Id, phoneNumber), userName);
        }

        public bool IsPhoneNumberConfirmed()
        {
            return InvokeUserManager((m, userId) => m.IsPhoneNumberConfirmedAsync(userId));
        }

        public async Task<bool> IsPhoneNumberConfirmedAsync()
        {
            return await InvokeUserManagerAsync((m, userId) => m.IsPhoneNumberConfirmedAsync(userId));
        }

        public bool IsPhoneNumberConfirmed(string userName)
        {
            return InvokeUserManager((m, u) => m.IsPhoneNumberConfirmedAsync(u.Id), userName);
        }

        public async Task<bool> IsPhoneNumberConfirmedAsync(string userName)
        {
            return await InvokeUserManagerAsync((m, u) => m.IsPhoneNumberConfirmedAsync(u.Id), userName);
        }

        // Logins

        public void AddLogin(string loginProvider, string providerKey)
        {
            InvokeUserManager((m, userId) => m.AddLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public async Task AddLoginAsync(string loginProvider, string providerKey)
        {
            await InvokeUserManagerAsync((m, userId) => m.AddLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public void AddLogin(string userName, string loginProvider, string providerKey)
        {
            InvokeUserManager((m, u) => m.AddLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }

        public async Task AddLoginAsync(string userName, string loginProvider, string providerKey)
        {
            await InvokeUserManagerAsync((m, u) => m.AddLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            InvokeUserManager((m, userId) => m.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public async Task RemoveLoginAsync(string loginProvider, string providerKey)
        {
            await InvokeUserManagerAsync((m, userId) => m.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public void RemoveLogin(string userName, string loginProvider, string providerKey)
        {
            InvokeUserManager((m, u) => m.RemoveLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }

        public async Task RemoveLoginAsync(string userName, string loginProvider, string providerKey)
        {
            await InvokeUserManagerAsync((m, u) => m.RemoveLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }

        // Claims

        public void AddClaim(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, userId) => m.AddClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(claimType, claimValue);
        }

        public async Task AddClaimAsync(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            await InvokeUserManagerAsync((m, userId) => m.AddClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(claimType, claimValue);
        }

        public void AddClaim(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, u) => m.AddClaimAsync(u.Id, claim), userName);
        }

        public async Task AddClaimAsync(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            await InvokeUserManagerAsync((m, u) => m.AddClaimAsync(u.Id, claim), userName);
        }

        public void SetClaim(string claimType, string claimValue)
        {
            InvokeUserManager(async (m, userId) =>
                                    {
                                        var user = await m.FindByIdAsync(userId);

                                        // Замена утверждения в коллекции пользователя
                                        var applicationUserClaims = user.Claims.Where(claim => !string.Equals(claim.Type.DisplayName, claimType, StringComparison.OrdinalIgnoreCase)).ToList();
                                        var newClaim = new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue };
                                        applicationUserClaims.Add(newClaim);
                                        user.Claims = applicationUserClaims;

                                        return await m.UpdateAsync(user);
                                    });

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.SetClaim(claimType, claimValue);
        }

        public async Task SetClaimAsync(string claimType, string claimValue)
        {
            await InvokeUserManagerAsync(async (m, userId) =>
            {
                var user = await m.FindByIdAsync(userId);

                // Замена утверждения в коллекции пользователя
                var applicationUserClaims = user.Claims.Where(claim => !string.Equals(claim.Type.DisplayName, claimType, StringComparison.OrdinalIgnoreCase)).ToList();
                var newClaim = new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue };
                applicationUserClaims.Add(newClaim);
                user.Claims = applicationUserClaims;

                return await m.UpdateAsync(user);
            });

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.SetClaim(claimType, claimValue);
        }

        public void SetClaim(string userName, string claimType, string claimValue)
        {
            InvokeUserManager(async (m, user) =>
                                    {
                                        // Замена утверждения в коллекции пользователя
                                        var applicationUserClaims = user.Claims.Where(claim => !string.Equals(claim.Type.DisplayName, claimType, StringComparison.OrdinalIgnoreCase)).ToList();
                                        var newClaim = new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue };
                                        applicationUserClaims.Add(newClaim);
                                        user.Claims = applicationUserClaims;

                                        return await m.UpdateAsync(user);
                                    }, userName);

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.SetClaim(claimType, claimValue);
        }

        public async Task SetClaimAsync(string userName, string claimType, string claimValue)
        {
            await InvokeUserManagerAsync(async (m, user) =>
            {
                // Замена утверждения в коллекции пользователя
                var applicationUserClaims = user.Claims.Where(claim => !string.Equals(claim.Type.DisplayName, claimType, StringComparison.OrdinalIgnoreCase)).ToList();
                var newClaim = new ApplicationUserClaim { Type = new ForeignKey { Id = claimType, DisplayName = claimType }, Value = claimValue };
                applicationUserClaims.Add(newClaim);
                user.Claims = applicationUserClaims;

                return await m.UpdateAsync(user);
            }, userName);

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.SetClaim(claimType, claimValue);
        }

        public void RemoveClaim(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, userId) => m.RemoveClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.RemoveClaims(claimType, v => string.Equals(claimValue, v));
        }

        public async Task RemoveClaimAsync(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            await InvokeUserManagerAsync((m, userId) => m.RemoveClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.RemoveClaims(claimType, v => string.Equals(claimValue, v));
        }

        public void RemoveClaim(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, u) => m.RemoveClaimAsync(u.Id, claim), userName);
        }

        public async Task RemoveClaimAsync(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            await InvokeUserManagerAsync((m, u) => m.RemoveClaimAsync(u.Id, claim), userName);
        }

        // Roles

        public bool IsInRole(string role)
        {
            return InvokeUserManager((m, userId) => m.IsInRoleAsync(userId, role));
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            return await InvokeUserManagerAsync((m, userId) => m.IsInRoleAsync(userId, role));
        }

        public bool IsInRole(string userName, string role)
        {
            return InvokeUserManager((m, u) => m.IsInRoleAsync(u.Id, role), userName);
        }

        public async Task<bool> IsInRoleAsync(string userName, string role)
        {
            return await InvokeUserManagerAsync((m, u) => m.IsInRoleAsync(u.Id, role), userName);
        }

        public void AddToRole(string role)
        {
            InvokeUserManager((m, userId) => m.AddToRoleAsync(userId, role));
            AddRolesToClaims(role);
        }

        public async Task AddToRoleAsync(string role)
        {
            await InvokeUserManagerAsync((m, userId) => m.AddToRoleAsync(userId, role));
            AddRolesToClaims(role);
        }

        public void AddToRole(string userName, string role)
        {
            InvokeUserManager((m, u) => m.AddToRoleAsync(u.Id, role), userName);
        }

        public async Task AddToRoleAsync(string userName, string role)
        {
            await InvokeUserManagerAsync((m, u) => m.AddToRoleAsync(u.Id, role), userName);
        }

        public void AddToRoles(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, roles));
                AddRolesToClaims(roles);
            }
        }

        public async Task AddToRolesAsync(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                await InvokeUserManagerAsync((m, userId) => m.AddToRolesAsync(userId, roles));
                AddRolesToClaims(roles);
            }
        }

        public void AddToRoles(string userName, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, u) => m.AddToRolesAsync(u.Id, roles), userName);
            }
        }

        public async Task AddToRolesAsync(string userName, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                await InvokeUserManagerAsync((m, u) => m.AddToRolesAsync(u.Id, roles), userName);
            }
        }

        public void AddToRoles(IEnumerable<string> roles)
        {
            var arrayRoles = roles?.ToArray();

            if (arrayRoles?.Length > 0)
            {
                InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, arrayRoles));
                AddRolesToClaims(arrayRoles);
            }
        }

        public async Task AddToRolesAsync(IEnumerable<string> roles)
        {
            var arrayRoles = roles?.ToArray();

            if (arrayRoles?.Length > 0)
            {
                await InvokeUserManagerAsync((m, userId) => m.AddToRolesAsync(userId, arrayRoles));
                AddRolesToClaims(arrayRoles);
            }
        }

        public void AddToRoles(string userName, IEnumerable<string> roles)
        {
            var arrayRoles = roles?.ToArray();

            if (arrayRoles?.Length > 0)
            {
                InvokeUserManager((m, u) => m.AddToRolesAsync(u.Id, arrayRoles), userName);
            }
        }

        public async Task AddToRolesAsync(string userName, IEnumerable<string> roles)
        {
            var arrayRoles = roles?.ToArray();

            if (arrayRoles?.Length > 0)
            {
                await InvokeUserManagerAsync((m, u) => m.AddToRolesAsync(u.Id, arrayRoles), userName);
            }
        }

        public void RemoveFromRole(string role)
        {
            InvokeUserManager((m, userId) => m.RemoveFromRoleAsync(userId, role));
            RemoveRolesFromClaims(role);
        }

        public async Task RemoveFromRoleAsync(string role)
        {
            await InvokeUserManagerAsync((m, userId) => m.RemoveFromRoleAsync(userId, role));
            RemoveRolesFromClaims(role);
        }

        public void RemoveFromRole(string userName, string role)
        {
            InvokeUserManager((m, u) => m.RemoveFromRoleAsync(u.Id, role), userName);
        }

        public async Task RemoveFromRoleAsync(string userName, string role)
        {
            await InvokeUserManagerAsync((m, u) => m.RemoveFromRoleAsync(u.Id, role), userName);
        }

        public void RemoveFromRoles(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, userId) => m.RemoveFromRolesAsync(userId, roles));
                RemoveRolesFromClaims(roles);
            }
        }

        public async Task RemoveFromRolesAsync(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                await InvokeUserManagerAsync((m, userId) => m.RemoveFromRolesAsync(userId, roles));
                RemoveRolesFromClaims(roles);
            }
        }

        public void RemoveFromRoles(IEnumerable<string> roles)
        {
            var rolesArray = roles?.ToArray();

            if (rolesArray?.Length > 0)
            {
                InvokeUserManager((m, userId) => m.RemoveFromRolesAsync(userId, rolesArray));
                RemoveRolesFromClaims(rolesArray);
            }
        }

        public async Task RemoveFromRolesAsync(IEnumerable<string> roles)
        {
            var rolesArray = roles?.ToArray();

            if (rolesArray?.Length > 0)
            {
                await InvokeUserManagerAsync((m, userId) => m.RemoveFromRolesAsync(userId, rolesArray));
                RemoveRolesFromClaims(rolesArray);
            }
        }


        // Helpers


        private delegate Task<T> ProcessCurrentUserFunc<T>(UserManager<IdentityApplicationUser> userManager, string currentUserId);

        private delegate Task<T> ProcessSpecifiedUserFunc<T>(UserManager<IdentityApplicationUser> userManager, IdentityApplicationUser specifiedUser);


        private TResult InvokeUserManager<TResult>(ProcessCurrentUserFunc<TResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            var result = AsyncHelper.RunSync(() => actionOnCurrentUser(UserManager, currentUserId));
            return result;
        }

        private async Task<TResult> InvokeUserManagerAsync<TResult>(ProcessCurrentUserFunc<TResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            return await actionOnCurrentUser(UserManager, currentUserId);
        }

        private void InvokeUserManager(ProcessCurrentUserFunc<IdentityResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            var result = AsyncHelper.RunSync(() => actionOnCurrentUser(UserManager, currentUserId));
            ThrowIfError(result);
        }

        private async Task InvokeUserManagerAsync(ProcessCurrentUserFunc<IdentityResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            var result = await actionOnCurrentUser(UserManager, currentUserId);
            ThrowIfError(result);
        }

        private TResult InvokeUserManager<TResult>(ProcessSpecifiedUserFunc<TResult> actionOnSpecifiedUser, string userName)
        {
            var result = AsyncHelper.RunSync(() => ProcessSpecifiedUser(actionOnSpecifiedUser, UserManager, userName));
            return result;
        }

        private async Task<TResult> InvokeUserManagerAsync<TResult>(ProcessSpecifiedUserFunc<TResult> actionOnSpecifiedUser, string userName)
        {
            return await ProcessSpecifiedUser(actionOnSpecifiedUser, UserManager, userName);
        }

        private void InvokeUserManager(ProcessSpecifiedUserFunc<IdentityResult> actionOnSpecifiedUser, string userName)
        {
            var result = AsyncHelper.RunSync(() => ProcessSpecifiedUser(actionOnSpecifiedUser, UserManager, userName));
            ThrowIfError(result);
        }

        private async Task InvokeUserManagerAsync(ProcessSpecifiedUserFunc<IdentityResult> actionOnSpecifiedUser, string userName)
        {
            var result = await ProcessSpecifiedUser(actionOnSpecifiedUser, UserManager, userName);
            ThrowIfError(result);
        }

        private static async Task<T> ProcessSpecifiedUser<T>(ProcessSpecifiedUserFunc<T> actionOnSpecifiedUser, UserManager<IdentityApplicationUser> userManager, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                throw new ArgumentException(Resources.UserNotFound, nameof(userName));
            }

            return await actionOnSpecifiedUser(userManager, user);
        }


        private string GetCurrentUserId()
        {
            var currentIdentity = GetCurrentIdentity();
            var currentUserId = currentIdentity.FindFirstClaim(ClaimTypes.NameIdentifier);
            return currentUserId;
        }

        private IIdentity GetCurrentIdentity()
        {
            var currentIdentity = _userIdentityProvider.GetUserIdentity();

            if (currentIdentity != null && !currentIdentity.IsAuthenticated)
            {
                throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
            }

            return currentIdentity;
        }

        private void AddRolesToClaims(params string[] roles)
        {
            var currentIdentity = GetCurrentIdentity();

            foreach (var role in roles)
            {
                currentIdentity.AddOrUpdateClaim(ClaimTypes.Role, role);
            }
        }

        private void RemoveRolesFromClaims(params string[] roles)
        {
            var currentIdentity = GetCurrentIdentity();
            currentIdentity.RemoveClaims(ClaimTypes.Role, roles.Contains);
        }

        private static void ThrowIfError(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    internal sealed class IdentityApplicationUserManager : IApplicationUserManager
    {
        private const int TaskTimeout = 60 * 1000;


        public IdentityApplicationUserManager(IUserIdentityProvider userIdentityProvider, UserManager<IdentityApplicationUser> userManager)
        {
            _userIdentityProvider = userIdentityProvider;
            _userManager = userManager;
        }


        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<IdentityApplicationUser> _userManager;


        public object GetCurrentUser()
        {
            return InvokeUserManager((m, userId) => m.FindByIdAsync(userId));
        }


        // CRUD

        public object FindUserByName(string userName)
        {
            return InvokeUserManager((m, u) => Task.FromResult(u), userName);
        }

        public void CreateUser(string userName, string password, string email = null)
        {
            ThrowIfError(_userManager.CreateAsync(new IdentityApplicationUser { UserName = userName, Email = email }, password));
        }

        public void DeleteUser(string userName)
        {
            InvokeUserManager((m, u) => m.DeleteAsync(u), userName);
        }


        // Password

        public bool HasPassword()
        {
            return InvokeUserManager((m, userId) => m.HasPasswordAsync(userId));
        }

        public bool HasPassword(string userName)
        {
            return InvokeUserManager((m, u) => m.HasPasswordAsync(u.Id), userName);
        }


        public void AddPassword(string password)
        {
            InvokeUserManager((m, userId) => m.AddPasswordAsync(userId, password));
        }

        public void AddPassword(string userName, string password)
        {
            InvokeUserManager((m, u) => m.AddPasswordAsync(u.Id, password), userName);
        }


        public void RemovePassword()
        {
            InvokeUserManager((m, userId) => m.RemovePasswordAsync(userId));
        }

        public void RemovePassword(string userName)
        {
            InvokeUserManager((m, u) => m.RemovePasswordAsync(u.Id), userName);
        }


        public void ChangePassword(string currentPassword, string newPassword)
        {
            InvokeUserManager((m, userId) => m.ChangePasswordAsync(userId, currentPassword, newPassword));
        }

        public void ChangePassword(string userName, string currentPassword, string newPassword)
        {
            InvokeUserManager((m, u) => m.ChangePasswordAsync(u.Id, currentPassword, newPassword), userName);
        }


        // SecurityStamp

        public string GetSecurityStamp()
        {
            return InvokeUserManager((m, userId) => m.GetSecurityStampAsync(userId));
        }

        public string GetSecurityStamp(string userName)
        {
            return InvokeUserManager((m, u) => m.GetSecurityStampAsync(u.Id), userName);
        }


        public void UpdateSecurityStamp()
        {
            InvokeUserManager((m, userId) => m.UpdateSecurityStampAsync(userId));
        }

        public void UpdateSecurityStamp(string userName)
        {
            InvokeUserManager((m, u) => m.UpdateSecurityStampAsync(u.Id), userName);
        }


        // Email

        public string GetEmail()
        {
            return InvokeUserManager((m, userId) => m.GetEmailAsync(userId));
        }

        public string GetEmail(string userName)
        {
            return InvokeUserManager((m, u) => m.GetEmailAsync(u.Id), userName);
        }


        public void SetEmail(string email)
        {
            InvokeUserManager((m, userId) => m.SetEmailAsync(userId, email));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.Email, email);
        }

        public void SetEmail(string userName, string email)
        {
            InvokeUserManager((m, u) => m.SetEmailAsync(u.Id, email), userName);
        }


        public bool IsEmailConfirmed()
        {
            return InvokeUserManager((m, userId) => m.IsEmailConfirmedAsync(userId));
        }

        public bool IsEmailConfirmed(string userName)
        {
            return InvokeUserManager((m, u) => m.IsEmailConfirmedAsync(u.Id), userName);
        }


        // PhoneNumber

        public string GetPhoneNumber()
        {
            return InvokeUserManager((m, userId) => m.GetPhoneNumberAsync(userId));
        }

        public string GetPhoneNumber(string userName)
        {
            return InvokeUserManager((m, u) => m.GetPhoneNumberAsync(u.Id), userName);
        }


        public void SetPhoneNumber(string phoneNumber)
        {
            InvokeUserManager((m, userId) => m.SetPhoneNumberAsync(userId, phoneNumber));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(ClaimTypes.MobilePhone, phoneNumber);
        }

        public void SetPhoneNumber(string userName, string phoneNumber)
        {
            InvokeUserManager((m, u) => m.SetPhoneNumberAsync(u.Id, phoneNumber), userName);
        }


        public bool IsPhoneNumberConfirmed()
        {
            return InvokeUserManager((m, userId) => m.IsPhoneNumberConfirmedAsync(userId));
        }

        public bool IsPhoneNumberConfirmed(string userName)
        {
            return InvokeUserManager((m, u) => m.IsPhoneNumberConfirmedAsync(u.Id), userName);
        }


        // Logins

        public void AddLogin(string loginProvider, string providerKey)
        {
            InvokeUserManager((m, userId) => m.AddLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public void AddLogin(string userName, string loginProvider, string providerKey)
        {
            InvokeUserManager((m, u) => m.AddLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }


        public void RemoveLogin(string loginProvider, string providerKey)
        {
            InvokeUserManager((m, userId) => m.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey)));
        }

        public void RemoveLogin(string userName, string loginProvider, string providerKey)
        {
            InvokeUserManager((m, u) => m.RemoveLoginAsync(u.Id, new UserLoginInfo(loginProvider, providerKey)), userName);
        }


        // Claims

        public void AddClaim(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, userId) => m.AddClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.AddOrUpdateClaim(claimType, claimValue);
        }

        public void AddClaim(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, u) => m.AddClaimAsync(u.Id, claim), userName);
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


        public void RemoveClaim(string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, userId) => m.RemoveClaimAsync(userId, claim));

            var currentIdentity = GetCurrentIdentity();
            currentIdentity.RemoveClaims(claimType, v => string.Equals(claimValue, v));
        }

        public void RemoveClaim(string userName, string claimType, string claimValue)
        {
            var claim = new Claim(claimType, claimValue);
            InvokeUserManager((m, u) => m.RemoveClaimAsync(u.Id, claim), userName);
        }


        // Roles

        public bool IsInRole(string role)
        {
            return InvokeUserManager((m, userId) => m.IsInRoleAsync(userId, role));
        }

        public bool IsInRole(string userName, string role)
        {
            return InvokeUserManager((m, u) => m.IsInRoleAsync(u.Id, role), userName);
        }


        public void AddToRole(string role)
        {
            InvokeUserManager((m, userId) => m.AddToRoleAsync(userId, role));
            AddRolesToClaims(role);
        }

        public void AddToRole(string userName, string role)
        {
            InvokeUserManager((m, u) => m.AddToRoleAsync(u.Id, role), userName);
        }


        public void AddToRoles(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, roles));
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


        public void AddToRoles(IEnumerable<string> roles)
        {
            var arrayRoles = roles?.ToArray();

            if (arrayRoles?.Length > 0)
            {
                InvokeUserManager((m, userId) => m.AddToRolesAsync(userId, arrayRoles));
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


        private void AddRolesToClaims(params string[] roles)
        {
            var currentIdentity = GetCurrentIdentity();

            foreach (var role in roles)
            {
                currentIdentity.AddOrUpdateClaim(ClaimTypes.Role, role);
            }
        }


        public void RemoveFromRole(string role)
        {
            InvokeUserManager((m, userId) => m.RemoveFromRoleAsync(userId, role));
            RemoveRolesFromClaims(role);
        }

        public void RemoveFromRole(string userName, string role)
        {
            InvokeUserManager((m, u) => m.RemoveFromRoleAsync(u.Id, role), userName);
        }


        public void RemoveFromRoles(params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, userId) => m.RemoveFromRolesAsync(userId, roles));
                RemoveRolesFromClaims(roles);
            }
        }

        public void RemoveFromRoles(string userName, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                InvokeUserManager((m, u) => m.RemoveFromRolesAsync(u.Id, roles), userName);
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

        public void RemoveFromRoles(string userName, IEnumerable<string> roles)
        {
            var rolesArray = roles?.ToArray();

            if (rolesArray?.Length > 0)
            {
                InvokeUserManager((m, u) => m.RemoveFromRolesAsync(u.Id, rolesArray), userName);
            }
        }


        private void RemoveRolesFromClaims(params string[] roles)
        {
            var currentIdentity = GetCurrentIdentity();
            currentIdentity.RemoveClaims(ClaimTypes.Role, roles.Contains);
        }


        // Helpers


        private delegate Task<T> ProcessCurrentUserFunc<T>(UserManager<IdentityApplicationUser> userManager, string currentUserId);

        private delegate Task<T> ProcessSpecifiedUserFunc<T>(UserManager<IdentityApplicationUser> userManager, IdentityApplicationUser specifiedUser);


        private TResult InvokeUserManager<TResult>(ProcessCurrentUserFunc<TResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            var task = actionOnCurrentUser(_userManager, currentUserId);
            return task.Result;
        }

        private void InvokeUserManager(ProcessCurrentUserFunc<IdentityResult> actionOnCurrentUser)
        {
            var currentUserId = GetCurrentUserId();
            var task = actionOnCurrentUser(_userManager, currentUserId);
            ThrowIfError(task);
        }

        private TResult InvokeUserManager<TResult>(ProcessSpecifiedUserFunc<TResult> actionOnSpecifiedUser, string userName)
        {
            var task = ProcessSpecifiedUser(actionOnSpecifiedUser, _userManager, userName);
            return task.Result;
        }

        private void InvokeUserManager(ProcessSpecifiedUserFunc<IdentityResult> actionOnSpecifiedUser, string userName)
        {
            var task = ProcessSpecifiedUser(actionOnSpecifiedUser, _userManager, userName);
            ThrowIfError(task);
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
            var currentIdentity = _userIdentityProvider.GetCurrentUserIdentity();
            var currentUserId = currentIdentity?.FindFirstClaim(ClaimTypes.NameIdentifier);
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);

            if (isNotAuthenticated)
            {
                throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
            }

            return currentIdentity;
        }


        private static void ThrowIfError(Task<IdentityResult> task)
        {
            if (!task.Wait(TaskTimeout))
            {
                throw new InvalidOperationException(Resources.OperationHasCancelledByTimeout);
            }

            if (!task.Result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, task.Result.Errors));
            }
        }
    }
}
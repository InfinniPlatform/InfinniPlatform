using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Security;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Сервис управления пользователями системы.
    /// </summary>
    internal class AuthManagementHttpService<TUser> : IHttpService where TUser : AppUser, new()
    {
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<TUser> _userManager;

        public AuthManagementHttpService(IUserIdentityProvider userIdentityProvider,
                                         UserManager<TUser> userManager)
        {
            _userIdentityProvider = userIdentityProvider;
            _userManager = userManager;
        }

        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";

            builder.Post["/GetCurrentUser"] = GetCurrentUser;
            builder.Post["/ChangePassword"] = ChangePassword;
        }

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        private async Task<object> GetCurrentUser(IHttpRequest request)
        {
            if (!Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var user = await GetUserInfo();

            var userInfo = BuildPublicUserInfo(user, Identity);

            return Extensions.CreateSuccesResponse(userInfo);
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        private async Task<object> ChangePassword(IHttpRequest request)
        {
            if (!Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var changePasswordForm = request.Form;
            string oldPassword = changePasswordForm.OldPassword;
            string newPassword = changePasswordForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return Extensions.CreateErrorResponse(Resources.NewPasswordCannotBeNullOrWhiteSpace, 400);
            }

            var user = await GetUserInfo();

            var changePasswordTask = string.IsNullOrEmpty(user.PasswordHash)
                                         ? await _userManager.AddPasswordAsync(user, newPassword)
                                         : await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!changePasswordTask.Succeeded)
            {
                var errorMessage = !changePasswordTask.Succeeded
                                       ? string.Join(Environment.NewLine, changePasswordTask.Errors)
                                       : null;

                return Extensions.CreateErrorResponse(errorMessage, 400);
            }

            return Extensions.CreateSuccesResponse<object>(null);
        }

        private async Task<TUser> GetUserInfo()
        {
            var userId = Identity.GetUserId();

            var userInfo = await _userManager.FindByIdAsync(userId);

            return userInfo;
        }

        private static PublicUserInfo BuildPublicUserInfo(TUser user, IIdentity identity)
        {
            var claims = new List<AppUserClaim>();

            if (user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    if (claim.Type != null && claim.Value != null)
                    {
                        claims.Add(claim);
                    }
                }
            }

            var claimsIdentity = identity as ClaimsIdentity;

            if (claimsIdentity?.Claims != null)
            {
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type != null && !claims.Exists(c => string.Equals(c.Type, claim.Type, StringComparison.OrdinalIgnoreCase)
                                                                  && string.Equals(c.Value, claim.Value, StringComparison.Ordinal)))
                    {
                        claims.Add(new AppUserClaim
                                   {
                                       Type = claim.Type,
                                       Value = claim.Value
                                   });
                    }
                }
            }

            return new PublicUserInfo(user.UserName, user.UserName, user.UserName, user.Roles, user.Logins, claims);
        }
    }
}
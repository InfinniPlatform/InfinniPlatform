using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using InfinniPlatform.Auth.HttpService.Models;
using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Security;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Auth.HttpService.Controllers
{
    /// <summary>
    /// Сервис управления пользователями системы.
    /// </summary>
    [Route("Auth")]
    public class AuthManagementController<TUser> : Controller where TUser : AppUser, new()
    {
        public AuthManagementController(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        private readonly UserManager<TUser> _userManager;

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        [HttpPost("GetCurrentUser")]
        public async Task<object> GetCurrentUser()
        {
            if (!HttpContext.User.Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var user = await GetUserInfo();

            var userInfo = BuildPublicUserInfo(user, HttpContext.User.Identity);

            return Extensions.CreateSuccesResponse(userInfo);
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        [HttpPost("ChangePassword")]
        public async Task<object> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            if (string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return Extensions.CreateErrorResponse(Resources.NewPasswordCannotBeNullOrWhiteSpace, 400);
            }

            var user = await GetUserInfo();

            var changePasswordTask = string.IsNullOrEmpty(user.PasswordHash)
                                         ? await _userManager.AddPasswordAsync(user, model.NewPassword)
                                         : await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

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
            var userId = HttpContext.User.Identity.GetUserId();

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

            return new PublicUserInfo(user, claims);
        }
    }
}
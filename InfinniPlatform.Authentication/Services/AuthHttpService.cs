using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Owin.Security;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace InfinniPlatform.Authentication.Services
{
    internal class AuthHttpService : IHttpService
    {
        public AuthHttpService(IOwinContextProvider owinContextProvider, IUserIdentityProvider userIdentityProvider)
        {
            _owinContextProvider = owinContextProvider;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly IOwinContextProvider _owinContextProvider;
        private readonly IUserIdentityProvider _userIdentityProvider;


        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        private IOwinContext OwinContext => _owinContextProvider.GetOwinContext();

        private IAuthenticationManager AuthenticationManager => OwinContext.Authentication;

        private UserManager<IdentityApplicationUser> ApplicationUserManager => OwinContext.GetUserManager<UserManager<IdentityApplicationUser>>();



        public void Load(IHttpServiceBuilder builder)
        {
            // TODO: Изменить базовый адрес после тестирования.
            builder.ServicePath = "/Auth2";

            // Методы работы с учетной записью пользователя
            builder.Post["/GetCurrentUser"] = GetCurrentUser;
            builder.Post["/ChangePassword"] = ChangePassword;

            // Методы входа пользователя в систему
            builder.Post["/SignInInternal"] = SignInInternal;
            builder.Post["/SignInExternal"] = SignInExternal;
            builder.Get["/SignInExternalCallback"] = SignInExternalCallback;

            // Методы для работы с внешними провайдерами аутентификации
            builder.Post["/GetExternalProviders"] = GetExternalProviders;
            builder.Post["/LinkExternalLogin"] = LinkExternalLogin;
            builder.Get["/LinkExternalLoginCallback"] = LinkExternalLoginCallback;
            builder.Post["/UnlinkExternalLogin"] = UnlinkExternalLogin;

            // Метод выхода из системы
            builder.Post["/SignOut"] = SignOut;
        }


        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        private async Task<object> GetCurrentUser(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return NotAuthenticated();
            }

            var user = await GetUserInfo();

            var userInfo = BuildSafeUserInfo(user);

            return userInfo;
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        private async Task<object> ChangePassword(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return NotAuthenticated();
            }

            dynamic changePasswordForm = request.Form;
            string oldPassword = changePasswordForm.OldPassword;
            string newPassword = changePasswordForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return BadRequest(Resources.NewPasswordCannotBeNullOrWhiteSpace);
            }

            var user = await GetUserInfo();

            var changePasswordTask = string.IsNullOrEmpty(user.PasswordHash)
                ? await ApplicationUserManager.AddPasswordAsync(user.Id, newPassword)
                : await ApplicationUserManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);

            return CreateRequest(changePasswordTask);
        }

        private Task<object> SignInInternal(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> SignInExternal(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> SignInExternalCallback(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> LinkExternalLogin(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> LinkExternalLoginCallback(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> UnlinkExternalLogin(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        private Task<object> SignOut(IHttpRequest request)
        {
            throw new NotImplementedException();
        }


        // HELPERS

        private bool IsAuthenticated()
        {
            return (Identity != null && Identity.IsAuthenticated);
        }

        private Task<IdentityApplicationUser> GetUserInfo()
        {
            var userId = SecurityExtensions.GetUserId(Identity);

            var userInfo = ApplicationUserManager.FindByIdAsync(userId);

            return userInfo;
        }

        private object BuildSafeUserInfo(IdentityApplicationUser user)
        {
            var claims = new List<ApplicationUserClaim>();

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

            var identity = Identity as ClaimsIdentity;

            if (identity != null && identity.Claims != null)
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type != null && !claims.Exists(c => string.Equals(c.Type.Id, claim.Type, StringComparison.OrdinalIgnoreCase) && string.Equals(c.Value, claim.Value, StringComparison.Ordinal)))
                    {
                        claims.Add(CreateUserClaim(claim));
                    }
                }
            }

            var safeUserInfo = new
            {
                user.UserName,
                user.DisplayName,
                user.Description,
                user.Roles,
                user.Logins,
                Claims = claims
            };

            return safeUserInfo;
        }

        private static ApplicationUserClaim CreateUserClaim(Claim claim)
        {
            return new ApplicationUserClaim
            {
                Type = new ForeignKey { Id = claim.Type },
                Value = claim.Value
            };
        }


        private static IHttpResponse NotAuthenticated()
        {
            // TODO: 401
            return new TextHttpResponse(Resources.RequestIsNotAuthenticated) { StatusCode = 400 };
        }

        private static IHttpResponse BadRequest(string message)
        {
            // TODO: Unify
            return new TextHttpResponse(message) { StatusCode = 400 };
        }

        private static IHttpResponse CreateRequest(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                return BadRequest(string.Join(Environment.NewLine, result.Errors));
            }

            return HttpResponse.Ok;
        }
    }
}
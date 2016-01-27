using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Owin;
using InfinniPlatform.Owin.Security;
using InfinniPlatform.Sdk.Logging;
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
            builder.ServicePath = "/Auth";

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

            var userInfo = BuildSafeUserInfo(user, Identity);

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

            return CreateResponse(changePasswordTask);
        }

        private async Task<object> SignInInternal(IHttpRequest request)
        {
            dynamic signInForm = request.Form;
            string userName = signInForm.UserName;
            string password = signInForm.Password;
            bool? remember = signInForm.Remember;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest(Resources.UserNameCannotBeNullOrWhiteSpace);
            }

            var user = await ApplicationUserManager.FindAsync(userName, password);

            if (user == null)
            {
                return BadRequest(Resources.InvalidUsernameOrPassword);
            }

            return SignIn(user, remember);
        }

        private Task<object> SignInExternal(IHttpRequest request)
        {
            return Task.FromResult<object>(ChallengeExternalProvider(request, "/SignInExternalCallback"));
        }

        private Task<object> SignInExternalCallback(IHttpRequest request)
        {
            return ChallengeExternalProviderCallback(request, async loginInfo =>
            {
                var user = await ApplicationUserManager.FindAsync(loginInfo.Login);

                // Если пользователь не найден в хранилище пользователей системы
                if (user == null)
                {
                    user = CreateUserByLoginInfo(loginInfo);

                    // Создание записи о пользователе
                    var createUserTask = await ApplicationUserManager.CreateAsync(user);

                    if (!createUserTask.Succeeded)
                    {
                        return BuildErrorMessage(createUserTask);
                    }

                    // Добавление имени входа пользователя
                    var addLoginTask = await ApplicationUserManager.AddLoginAsync(user.Id, loginInfo.Login);

                    if (!addLoginTask.Succeeded)
                    {
                        return BuildErrorMessage(createUserTask);
                    }
                }

                await SignIn(user, false);

                return null;
            });
        }

        private Task<object> LinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return Task.FromResult<object>(NotAuthenticated());
            }

            return Task.FromResult<object>(ChallengeExternalProvider(request, "/LinkExternalLoginCallback"));
        }

        private Task<object> LinkExternalLoginCallback(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return Task.FromResult<object>(NotAuthenticated());
            }

            return ChallengeExternalProviderCallback(request, async loginInfo =>
            {
                // Определение текущего пользователя
                var userId = SecurityExtensions.GetUserId(Identity);

                // Добавление имени входа пользователя
                var addLoginTask = await ApplicationUserManager.AddLoginAsync(userId, loginInfo.Login);

                return BuildErrorMessage(addLoginTask);
            });
        }

        private async Task<object> UnlinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return NotAuthenticated();
            }

            dynamic unlinkExternalLoginForm = request.Form;
            string provider = unlinkExternalLoginForm.Provider;
            string providerKey = unlinkExternalLoginForm.ProviderKey;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest(Resources.ExternalProviderCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return BadRequest(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace);
            }

            // Определение текущего пользователя
            var userId = SecurityExtensions.GetUserId(Identity);

            // Удаление имени входа пользователя
            var removeLoginTask = await ApplicationUserManager.RemoveLoginAsync(userId, new UserLoginInfo(provider, providerKey));

            return CreateResponse(removeLoginTask);
        }

        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            var loginProviders = AuthenticationManager.GetExternalAuthenticationTypes();
            var loginProvidersList = (loginProviders != null) ? loginProviders.Select(i => new { Type = i.AuthenticationType, Name = i.Caption }).ToArray() : null;
            return Task.FromResult<object>(CreateResponse(loginProvidersList));
        }

        private Task<object> SignOut(IHttpRequest request)
        {
            // Выход из системы
            AuthenticationManager.SignOut();

            return Task.FromResult<object>(HttpResponse.Ok);
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


        private async Task<object> SignIn(IdentityApplicationUser user, bool? remember)
        {
            // Выход из системы
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            // Создание новых учетных данных
            var identity = await ApplicationUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Вход в систему с новыми учетными данными
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = (remember == true) }, identity);

            var userInfo = BuildSafeUserInfo(user, identity);

            return CreateResponse(userInfo);
        }

        private static IdentityApplicationUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var userName = loginInfo.DefaultUserName;

            if (loginInfo.Login != null)
            {
                userName = $"{loginInfo.Login.LoginProvider}{loginInfo.Login.ProviderKey}".Replace(" ", "");
            }

            var user = new IdentityApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = loginInfo.Email,
                EmailConfirmed = !string.IsNullOrWhiteSpace(loginInfo.Email)
            };

            if (loginInfo.ExternalIdentity != null && loginInfo.ExternalIdentity.Claims != null)
            {
                user.Claims = loginInfo.ExternalIdentity.Claims.Select(CreateUserClaim);
            }

            return user;
        }

        /// <summary>
        /// Осуществляет переход на страницу входа внешнего провайдера.
        /// </summary>
        private IHttpResponse ChallengeExternalProvider(IHttpRequest request, string callbackPath)
        {
            dynamic signInForm = request.Form;
            string provider = signInForm.Provider;
            string successUrl = signInForm.SuccessUrl;
            string failureUrl = signInForm.FailureUrl;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest(Resources.ExternalProviderCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(successUrl))
            {
                return BadRequest(Resources.SuccessUrlCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(failureUrl))
            {
                return BadRequest(Resources.FailureUrlCannotBeNullOrWhiteSpace);
            }

            // Адрес возврата для приема подтверждения от внешнего провайдера
            var callbackUri = new UrlBuilder()
                .Relative(callbackPath)
                .AddQuery("SuccessUrl", successUrl)
                .AddQuery("FailureUrl", failureUrl)
                .ToString();

            // Перенаправление пользователя на страницу входа внешнего провайдера
            var authProperties = new AuthenticationProperties { RedirectUri = callbackUri };
            AuthenticationManager.Challenge(authProperties, provider);

            var response = CreateResponse(OwinContext.Response);

            return response;
        }

        private async Task<object> ChallengeExternalProviderCallback(IHttpRequest request, Func<ExternalLoginInfo, Task<string>> callbackAction)
        {
            var successUrl = request.Query.SuccessUrl;
            var failureUrl = request.Query.FailureUrl;

            string errorMessage;

            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                // Если пользователь прошел аутентификацию через внешний провайдер
                if (loginInfo != null)
                {
                    errorMessage = await callbackAction(loginInfo);
                }
                else
                {
                    errorMessage = Resources.UnsuccessfulSignInWithExternalProvider;
                }
            }
            catch (AggregateException error)
            {
                errorMessage = error.GetFullMessage();
            }

            // Перенаправление пользователя на страницу приложения

            RedirectHttpResponse response;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response = new RedirectHttpResponse(new UrlBuilder(failureUrl).AddQuery("error", errorMessage).ToString());
                response.SetHeader("Warning", errorMessage);
            }
            else
            {
                response = new RedirectHttpResponse(successUrl);
            }

            return response;
        }


        private static object BuildSafeUserInfo(IdentityApplicationUser user, IIdentity identity)
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

            var claimsIdentity = identity as ClaimsIdentity;

            if (claimsIdentity != null && claimsIdentity.Claims != null)
            {
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type != null && !claims.Exists(c => string.Equals(c.Type.Id, claim.Type, StringComparison.OrdinalIgnoreCase)
                                                                  && string.Equals(c.Value, claim.Value, StringComparison.Ordinal)))
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

        private static IHttpResponse CreateResponse(object result)
        {
            if (result != null)
            {
                return new JsonHttpResponse(result) { StatusCode = 200 };
            }

            return HttpResponse.Ok;
        }

        private static IHttpResponse CreateResponse(IOwinResponse response)
        {
            return new HttpResponse(response.StatusCode, response.ContentType)
            {
                ReasonPhrase = response.ReasonPhrase,
                Headers = response.Headers.ToDictionary(i => i.Key, kv => string.Join(";", kv.Value))
            };
        }

        private static IHttpResponse CreateResponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errorMessage = BuildErrorMessage(result);

                return BadRequest(errorMessage);
            }

            return HttpResponse.Ok;
        }

        private static string BuildErrorMessage(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                return string.Join(Environment.NewLine, result.Errors);
            }

            return null;
        }
    }
}
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
    /// <summary>
    /// Сервис аутентификации пользователей системы.
    /// </summary>
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

            // Методы работы с учетной записью
            builder.Post["/GetCurrentUser"] = GetCurrentUser; //safeuserinfo
            builder.Post["/ChangePassword"] = ChangePassword; //TextHttpResponse or HttpResponce.Ok

            // Методы входа и выхода в систему
            builder.Post["/SignInInternal"] = SignInInternal; //TextHttpRespnce or JsonHttpResponse
            builder.Post["/SignInExternal"] = SignInExternal; //TextHttpResponse or new HttpResponce
            builder.Get["/SignInExternalCallback"] = SignInExternalCallback; //callback
            builder.Post["/SignOut"] = SignOut; // new HttpResponse

            // Методы для работы с внешними провайдерами аутентификации
            builder.Post["/GetExternalProviders"] = GetExternalProviders; //JsonHttpResponse or HttpResponse.Ok
            builder.Post["/LinkExternalLogin"] = LinkExternalLogin; //TextHttpResponse or new HttpResponse
            builder.Get["/LinkExternalLoginCallback"] = LinkExternalLoginCallback; //callback
            builder.Post["/UnlinkExternalLogin"] = UnlinkExternalLogin; //TextHttpResponse or HttpResponse.Ok
        }

        // ACCOUNT

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        private async Task<object> GetCurrentUser(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return new TextHttpResponse(Resources.RequestIsNotAuthenticated) { StatusCode = 401 };
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
                return new TextHttpResponse(Resources.RequestIsNotAuthenticated) { StatusCode = 401 };
            }

            dynamic changePasswordForm = request.Form;
            string oldPassword = changePasswordForm.OldPassword;
            string newPassword = changePasswordForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return new TextHttpResponse(Resources.NewPasswordCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            var user = await GetUserInfo();

            var changePasswordTask = string.IsNullOrEmpty(user.PasswordHash)
                                         ? await ApplicationUserManager.AddPasswordAsync(user.Id, newPassword)
                                         : await ApplicationUserManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);

            if (!changePasswordTask.Succeeded)
            {
                var errorMessage = !changePasswordTask.Succeeded
                                       ? string.Join(Environment.NewLine, changePasswordTask.Errors)
                                       : null;

                return new TextHttpResponse(errorMessage) { StatusCode = 400 };
            }

            return HttpResponse.Ok;
        }

        // SIGNIN & SIGNOUT

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        private async Task<object> SignInInternal(IHttpRequest request)
        {
            dynamic signInForm = request.Form;
            string userName = signInForm.UserName;
            string password = signInForm.Password;
            bool? remember = signInForm.Remember;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return new TextHttpResponse(Resources.UserNameCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            var user = await ApplicationUserManager.FindAsync(userName, password);

            if (user == null)
            {
                return new TextHttpResponse(Resources.InvalidUsernameOrPassword) { StatusCode = 400 };
            }

            var signInInternal = await SignIn(user, remember);

            return signInInternal;
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внешний провайдер.
        /// </summary>
        private Task<object> SignInExternal(IHttpRequest request)
        {
            var challengeResult = ChallengeExternalProvider(request, "/Auth/SignInExternalCallback");

            return Task.FromResult<object>(challengeResult);
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
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
                                                                                return !createUserTask.Succeeded
                                                                                           ? string.Join(Environment.NewLine, createUserTask.Errors)
                                                                                           : null;
                                                                            }

                                                                            // Добавление имени входа пользователя
                                                                            var addLoginTask = await ApplicationUserManager.AddLoginAsync(user.Id, loginInfo.Login);

                                                                            if (!addLoginTask.Succeeded)
                                                                            {
                                                                                return !createUserTask.Succeeded
                                                                                           ? string.Join(Environment.NewLine, createUserTask.Errors)
                                                                                           : null;
                                                                            }
                                                                        }

                                                                        await SignIn(user, false);

                                                                        return null;
                                                                    });
        }

        /// <summary>
        /// Осуществляет вход указанного пользователя в систему.
        /// </summary>
        private async Task<object> SignIn(IdentityApplicationUser user, bool? remember)
        {
            // Выход из системы
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            // Создание новых учетных данных
            var identity = await ApplicationUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Вход в систему с новыми учетными данными
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = remember == true }, identity);

            var userInfo = BuildSafeUserInfo(user, identity);

            return userInfo != null
                       ? new JsonHttpResponse(userInfo) { StatusCode = 200 }
                       : HttpResponse.Ok;
        }

        /// <summary>
        /// Создает учетную запись пользователя по информации внешнего провайдера.
        /// </summary>
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

            if (loginInfo.ExternalIdentity?.Claims != null)
            {
                user.Claims = loginInfo.ExternalIdentity.Claims.Select(CreateUserClaim);
            }

            return user;
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private Task<object> SignOut(IHttpRequest request)
        {
            // Выход из системы
            AuthenticationManager.SignOut();

            var owinResponse = OwinContext.Response;
            var response = new HttpResponse(owinResponse.StatusCode, owinResponse.ContentType)
                           {
                               ReasonPhrase = owinResponse.ReasonPhrase,
                               Headers = owinResponse.Headers.ToDictionary(i => i.Key, kv => string.Join(";", kv.Value))
                           };

            return Task.FromResult<object>(response);
        }

        // EXTERNAL PROVIDERS

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            var loginProviders = AuthenticationManager.GetExternalAuthenticationTypes();
            var loginProvidersList = loginProviders?.Select(i => new { Type = i.AuthenticationType, Name = i.Caption }).ToArray();

            var httpResponse = loginProvidersList != null
                                   ? new JsonHttpResponse(loginProvidersList) { StatusCode = 200 }
                                   : HttpResponse.Ok;

            return Task.FromResult<object>(httpResponse);
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        private Task<object> LinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return Task.FromResult<object>(new TextHttpResponse(Resources.RequestIsNotAuthenticated) { StatusCode = 401 });
            }

            var challengeResult = ChallengeExternalProvider(request, "/Auth/LinkExternalLoginCallback");

            return Task.FromResult<object>(challengeResult);
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private Task<object> LinkExternalLoginCallback(IHttpRequest request)
        {
            return ChallengeExternalProviderCallback(request, async loginInfo =>
                                                                    {
                                                                        // Определение текущего пользователя
                                                                        var userId = SecurityExtensions.GetUserId(Identity);

                                                                        // Добавление имени входа пользователя
                                                                        var addLoginTask = await ApplicationUserManager.AddLoginAsync(userId, loginInfo.Login);

                                                                        return !addLoginTask.Succeeded
                                                                                   ? string.Join(Environment.NewLine, addLoginTask.Errors)
                                                                                   : null;
                                                                    });
        }

        /// <summary>
        /// Удаляет у текущего пользователя имя входа у внешнего провайдера.
        /// </summary>
        private async Task<object> UnlinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return new TextHttpResponse(Resources.RequestIsNotAuthenticated) { StatusCode = 401 };
            }

            dynamic unlinkExternalLoginForm = request.Form;
            string provider = unlinkExternalLoginForm.Provider;
            string providerKey = unlinkExternalLoginForm.ProviderKey;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return new TextHttpResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return new TextHttpResponse(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            // Определение текущего пользователя
            var userId = SecurityExtensions.GetUserId(Identity);

            // Удаление имени входа пользователя
            var removeLoginTask = await ApplicationUserManager.RemoveLoginAsync(userId, new UserLoginInfo(provider, providerKey));

            if (!removeLoginTask.Succeeded)
            {
                var errorMessage = !removeLoginTask.Succeeded
                                       ? string.Join(Environment.NewLine, removeLoginTask.Errors)
                                       : null;

                return new TextHttpResponse(errorMessage) { StatusCode = 400 };
            }

            return HttpResponse.Ok;
        }

        // CHALLENGE

        /// <summary>
        /// Осуществляет переход на страницу входа внешнего провайдера.
        /// </summary>
        private IHttpResponse ChallengeExternalProvider(IHttpRequest request, string callbackPath)
        {
            dynamic challengeForm = request.Form;
            string provider = challengeForm.Provider;
            string successUrl = challengeForm.SuccessUrl;
            string failureUrl = challengeForm.FailureUrl;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return new TextHttpResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            if (string.IsNullOrWhiteSpace(successUrl))
            {
                return new TextHttpResponse(Resources.SuccessUrlCannotBeNullOrWhiteSpace) { StatusCode = 400 };
            }

            if (string.IsNullOrWhiteSpace(failureUrl))
            {
                return new TextHttpResponse(Resources.FailureUrlCannotBeNullOrWhiteSpace) { StatusCode = 400 };
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

            var response1 = OwinContext.Response;
            var response = new HttpResponse(response1.StatusCode, response1.ContentType)
                           {
                               ReasonPhrase = response1.ReasonPhrase,
                               Headers = response1.Headers.ToDictionary(i => i.Key, kv => string.Join(";", kv.Value))
                           };

            return response;
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
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

            if (string.IsNullOrEmpty(errorMessage))
            {
                response = new RedirectHttpResponse(successUrl);
            }
            else
            {
                response = new RedirectHttpResponse(new UrlBuilder(failureUrl).AddQuery("error", errorMessage).ToString());
                response.SetHeader("Warning", errorMessage);
            }

            return response;
        }

        // USER INFO

        private bool IsAuthenticated()
        {
            return Identity != null && Identity.IsAuthenticated;
        }

        private Task<IdentityApplicationUser> GetUserInfo()
        {
            var userId = SecurityExtensions.GetUserId(Identity);

            var userInfo = ApplicationUserManager.FindByIdAsync(userId);

            return userInfo;
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

            if (claimsIdentity?.Claims != null)
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

            return new SafeUserInfo(user.UserName, user.DisplayName, user.Description, user.Roles, user.Logins, claims);
        }

        private static ApplicationUserClaim CreateUserClaim(Claim claim)
        {
            return new ApplicationUserClaim
                   {
                       Type = new ForeignKey { Id = claim.Type },
                       Value = claim.Value
                   };
        }
    }
}
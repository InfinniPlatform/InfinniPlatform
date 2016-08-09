using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Properties;
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
        public AuthHttpService(IOwinContextProvider owinContextProvider, IUserIdentityProvider userIdentityProvider, UserEventHandlerInvoker userEventHandlerInvoker)
        {
            _owinContextProvider = owinContextProvider;
            _userIdentityProvider = userIdentityProvider;
            _userEventHandlerInvoker = userEventHandlerInvoker;
        }


        private readonly IOwinContextProvider _owinContextProvider;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;


        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        private IOwinContext OwinContext => _owinContextProvider.GetOwinContext();

        private IAuthenticationManager AuthenticationManager => OwinContext.Authentication;

        private UserManager<IdentityApplicationUser> ApplicationUserManager => OwinContext.GetUserManager<UserManager<IdentityApplicationUser>>();


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";

            // Методы работы с учетной записью
            builder.Post["/GetCurrentUser"] = GetCurrentUser;
            builder.Post["/ChangePassword"] = ChangePassword;

            // Методы входа и выхода в систему
            builder.Post["/SignInInternal"] = SignInInternal;
            builder.Post["/SignInExternal"] = SignInExternal;
            builder.Get["/SignInExternalCallback"] = SignInExternalCallback;
            builder.Post["/SignOut"] = SignOut;

            // Методы для работы с внешними провайдерами аутентификации
            builder.Post["/GetExternalProviders"] = GetExternalProviders;
            builder.Post["/LinkExternalLogin"] = LinkExternalLogin;
            builder.Get["/LinkExternalLoginCallback"] = LinkExternalLoginCallback;
            builder.Post["/UnlinkExternalLogin"] = UnlinkExternalLogin;
        }


        // ACCOUNT

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        private async Task<object> GetCurrentUser(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var user = await GetUserInfo();

            var userInfo = BuildPublicUserInfo(user, Identity);

            return CreateSuccesResponse(userInfo);
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        private async Task<object> ChangePassword(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            dynamic changePasswordForm = request.Form;
            string oldPassword = changePasswordForm.OldPassword;
            string newPassword = changePasswordForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return CreateErrorResponse(Resources.NewPasswordCannotBeNullOrWhiteSpace, 400);
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

                return CreateErrorResponse(errorMessage, 400);
            }

            return CreateSuccesResponse<object>(null);
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
                return CreateErrorResponse(Resources.UserNameCannotBeNullOrWhiteSpace, 400);
            }

            var user = await ApplicationUserManager.FindAsync(userName, password);

            if (user == null)
            {
                return CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);
            }

            var userInfo = await SignIn(user, remember);

            return CreateSuccesResponse(userInfo);
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
        private async Task<PublicUserInfo> SignIn(IdentityApplicationUser user, bool? remember)
        {
            // Выход из системы
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            // Создание новых учетных данных
            var identity = await ApplicationUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            // Вход в систему с новыми учетными данными
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = remember == true }, identity);

            // Вызов обработчика события входа пользователя
            _userEventHandlerInvoker.OnAfterSignIn(identity);

            var userInfo = BuildPublicUserInfo(user, identity);

            return userInfo;
        }

        /// <summary>
        /// Создает учетную запись пользователя по информации внешнего провайдера.
        /// </summary>
        private static IdentityApplicationUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var userName = loginInfo.DefaultUserName;

            if (loginInfo.Login != null)
            {
                userName = $"{loginInfo.Login.LoginProvider}{loginInfo.Login.ProviderKey}".Replace(" ", string.Empty);
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
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(request.User);

            // Выход из системы
            AuthenticationManager.SignOut();

            var owinResponse = OwinContext.Response;

            var response = new JsonHttpResponse(new ServiceResult<object> { Success = true }) { StatusCode = owinResponse.StatusCode };

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

            var httpResponse = CreateSuccesResponse(loginProvidersList);

            return Task.FromResult<object>(httpResponse);
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        private Task<object> LinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                var response = CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
                return Task.FromResult<object>(response);
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
                return CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            dynamic unlinkExternalLoginForm = request.Form;
            string provider = unlinkExternalLoginForm.Provider;
            string providerKey = unlinkExternalLoginForm.ProviderKey;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
            }

            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return CreateErrorResponse(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace, 400);
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

                return CreateErrorResponse(errorMessage, 400);
            }

            return CreateSuccesResponse<object>(null);
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
                return CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
            }

            if (string.IsNullOrWhiteSpace(successUrl))
            {
                return CreateErrorResponse(Resources.SuccessUrlCannotBeNullOrWhiteSpace, 400);
            }

            if (string.IsNullOrWhiteSpace(failureUrl))
            {
                return CreateErrorResponse(Resources.FailureUrlCannotBeNullOrWhiteSpace, 400);
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

            var owinResponse = OwinContext.Response;
            var response = new HttpResponse(owinResponse.StatusCode, owinResponse.ContentType)
            {
                ReasonPhrase = owinResponse.ReasonPhrase,
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

        private static PublicUserInfo BuildPublicUserInfo(ApplicationUser user, IIdentity identity)
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

            return new PublicUserInfo(user.UserName, user.DisplayName, user.Description, user.Roles, user.Logins, claims);
        }

        private static ApplicationUserClaim CreateUserClaim(Claim claim)
        {
            return new ApplicationUserClaim
            {
                Type = new ForeignKey { Id = claim.Type },
                Value = claim.Value
            };
        }

        //RESPONSE

        private static JsonHttpResponse CreateErrorResponse(string errorMessage, int statusCode)
        {
            var errorResponse = new JsonHttpResponse(new ServiceResult<object> { Success = false, Error = errorMessage }) { StatusCode = statusCode };
            return errorResponse;
        }

        private static JsonHttpResponse CreateSuccesResponse<TResult>(TResult result) where TResult : class
        {
            var successResponse = new JsonHttpResponse(new ServiceResult<TResult> { Success = true, Result = result }) { StatusCode = 200 };
            return successResponse;
        }

        // USER INFO

        /// <summary>
        /// Информация о пользователе, доступная через AuthHttpService.
        /// </summary>
        internal class PublicUserInfo
        {
            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="userName">Имя пользователя.</param>
            /// <param name="displayName">Отображаемое имя пользователя.</param>
            /// <param name="description">Описание.</param>
            /// <param name="roles">Роли пользователя.</param>
            /// <param name="logins">Учетные записи пользователя.</param>
            /// <param name="claims">Утверждения пользователя.</param>
            public PublicUserInfo(string userName,
                                  string displayName,
                                  string description,
                                  IEnumerable<ForeignKey> roles,
                                  IEnumerable<ApplicationUserLogin> logins,
                                  List<ApplicationUserClaim> claims)
            {
                UserName = userName;
                DisplayName = displayName;
                Description = description;
                Roles = roles;
                Logins = logins;
                Claims = claims;
            }

            public string UserName { get; set; }

            public string DisplayName { get; set; }

            public string Description { get; set; }

            public IEnumerable<ForeignKey> Roles { get; set; }

            public IEnumerable<ApplicationUserLogin> Logins { get; set; }

            public List<ApplicationUserClaim> Claims { get; set; }
        }
    }
}
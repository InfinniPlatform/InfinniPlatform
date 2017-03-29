using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Internal.Contract;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
using InfinniPlatform.Auth.Internal.Properties;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using HttpResponse = InfinniPlatform.Sdk.Http.Services.HttpResponse;

namespace InfinniPlatform.Auth.Internal.Services
{
    /// <summary>
    /// Сервис аутентификации пользователей системы.
    /// </summary>
    internal class AuthInternalHttpService : IHttpService
    {
        private const string ApplicationCookieScheme = "ApplicationCookie";
        private const string ExternalCookieScheme = "ExternalCookie";


        private readonly IHttpContextProvider _httpContextProvider;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthInternalHttpService(IHttpContextProvider httpContextProvider,
                                       IUserIdentityProvider userIdentityProvider,
                                       UserEventHandlerInvoker userEventHandlerInvoker,
                                       UserManager<IdentityUser> userManager,
                                       SignInManager<IdentityUser> signInManager)
        {
            _httpContextProvider = httpContextProvider;
            _userIdentityProvider = userIdentityProvider;
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        private HttpContext HttpContext => _httpContextProvider.GetHttpContext();

        private AuthenticationManager AuthenticationManager => HttpContext.Authentication;


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
                                         ? await _userManager.AddPasswordAsync(user, newPassword)
                                         : await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

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
            bool remember = ((bool?) signInForm.Remember).GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(userName))
            {
                return CreateErrorResponse(Resources.UserNameCannotBeNullOrWhiteSpace, 400);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                var signInResult = await _signInManager.PasswordSignInAsync(userName, password, remember, false);
                
                if (user == null)
                {
                    return CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);
                }

                return CreateSuccesResponse(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            var prevIdentity = request.User;

            return ChallengeExternalProviderCallback(request, async loginInfo =>
                                                              {
                                                                  var user = await _userManager.GetUserAsync(loginInfo.Principal);

                                                                  // Если пользователь не найден в хранилище пользователей системы
                                                                  if (user == null)
                                                                  {
                                                                      user = CreateUserByLoginInfo(loginInfo);

                                                                      // Создание записи о пользователе
                                                                      var createUserTask = await _userManager.CreateAsync(user);

                                                                      if (!createUserTask.Succeeded)
                                                                      {
                                                                          return !createUserTask.Succeeded
                                                                                     ? string.Join(Environment.NewLine, createUserTask.Errors)
                                                                                     : null;
                                                                      }

                                                                      // Добавление имени входа пользователя
                                                                      var addLoginTask = await _userManager.AddLoginAsync(user, loginInfo);

                                                                      if (!addLoginTask.Succeeded)
                                                                      {
                                                                          return !createUserTask.Succeeded
                                                                                     ? string.Join(Environment.NewLine, createUserTask.Errors)
                                                                                     : null;
                                                                      }
                                                                  }

                                                                  await SignIn(prevIdentity, user, false);

                                                                  return null;
                                                              });
        }

        /// <summary>
        /// Осуществляет вход указанного пользователя в систему.
        /// </summary>
        private async Task<PublicUserInfo> SignIn(IIdentity prevIdentity, IdentityUser user, bool? remember)
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(prevIdentity);

            // Выход из системы
            await AuthenticationManager.SignOutAsync(ExternalCookieScheme);

            // Создание новых учетных данных
            var identity = await _userManager.CreateAsync(user, ApplicationCookieScheme);

            // Вход в систему с новыми учетными данными
            await AuthenticationManager.SignInAsync(ApplicationCookieScheme, ClaimsPrincipal.Current, new AuthenticationProperties {IsPersistent = remember == true});

            // Вызов обработчика события входа пользователя
            // TODO Change method signature.
            //_userEventHandlerInvoker.OnAfterSignIn(identity);

            // TODO Change method signature.
            //            var userInfo = BuildPublicUserInfo(user, identity);
            //
            //            return userInfo;
            return null;
        }

        /// <summary>
        /// Создает учетную запись пользователя по информации внешнего провайдера.
        /// </summary>
        private static IdentityUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var userName = loginInfo.Principal.Identity.Name;
//
//            if (loginInfo.Login != null)
//            {
//                userName = $"{loginInfo.Login.LoginProvider}{loginInfo.Login.ProviderKey}".Replace(" ", string.Empty);
//            }

            var user = new IdentityUser
                       {
                           Id = Guid.NewGuid().ToString(),
                           UserName = userName
//                           Email = loginInfo.Email,
//                           EmailConfirmed = !string.IsNullOrWhiteSpace(loginInfo.Email)
                       };

//            if (loginInfo.ExternalIdentity?.Claims != null)
//            {
//                user.Claims = loginInfo.ExternalIdentity.Claims.Select(CreateUserClaim);
//            }

            return user;
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private async Task<object> SignOut(IHttpRequest request)
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(request.User);

            // Выход из системы
            await AuthenticationManager.SignOutAsync(ApplicationCookieScheme);

            var owinResponse = HttpContext.Response;

            var response = new JsonHttpResponse(new ServiceResult<object> {Success = true}) {StatusCode = owinResponse.StatusCode};

            return Task.FromResult<object>(response);
        }

        // EXTERNAL PROVIDERS

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            var loginProviders = AuthenticationManager.GetAuthenticationSchemes();
            var loginProvidersList = loginProviders?.Select(i => new {Type = i.AuthenticationScheme, Name = i.DisplayName}).ToArray();

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
                                                                  var identityUser = await _userManager.GetUserAsync(ClaimsPrincipal.Current);

                                                                  // Добавление имени входа пользователя
                                                                  var addLoginTask = await _userManager.AddLoginAsync(identityUser, new UserLoginInfo(loginInfo.LoginProvider, loginInfo.ProviderKey, loginInfo.ProviderDisplayName));

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

            var identityUser = await _userManager.GetUserAsync(ClaimsPrincipal.Current);

            // Удаление имени входа пользователя
            var removeLoginTask = await _userManager.RemoveLoginAsync(identityUser, provider, providerKey);

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
            var authProperties = new AuthenticationProperties {RedirectUri = callbackUri};
            AuthenticationManager.ChallengeAsync(authProperties);

            var owinResponse = HttpContext.Response;
            var response = new HttpResponse(owinResponse.StatusCode, owinResponse.ContentType);

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
                var loginInfo = await AuthenticationManager.GetAuthenticateInfoAsync(ExternalCookieScheme);

                // Если пользователь прошел аутентификацию через внешний провайдер
                if (loginInfo != null)
                {
                    // TODO Get correct external login info.
                    errorMessage = await callbackAction(new ExternalLoginInfo(loginInfo.Principal, loginInfo.Description.DisplayName, loginInfo.Description.DisplayName, loginInfo.Description.DisplayName));
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

        private Task<IdentityUser> GetUserInfo()
        {
            var userId = Identity.GetUserId();

            var userInfo = _userManager.FindByIdAsync(userId);

            return userInfo;
        }

        private static PublicUserInfo BuildPublicUserInfo(IdentityUser user, IIdentity identity)
        {
            var claims = new List<IdentityUserClaim>();

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
                        claims.Add(CreateUserClaim(claim));
                    }
                }
            }

            return new PublicUserInfo(user.UserName, user.UserName, user.UserName, user.Roles, user.Logins, claims);
        }

        private static IdentityUserClaim CreateUserClaim(Claim claim)
        {
            return new IdentityUserClaim
                   {
                       Type = claim.Type,
                       Value = claim.Value
                   };
        }

        // RESPONSE

        private static JsonHttpResponse CreateErrorResponse(string errorMessage, int statusCode)
        {
            var errorResponse = new JsonHttpResponse(new ServiceResult<object> {Success = false, Error = errorMessage}) {StatusCode = statusCode};
            return errorResponse;
        }

        private static JsonHttpResponse CreateSuccesResponse<TResult>(TResult result) where TResult : class
        {
            var successResponse = new JsonHttpResponse(new ServiceResult<TResult> {Success = true, Result = result}) {StatusCode = 200};
            return successResponse;
        }

        // USER INFO

        /// <summary>
        /// Информация о пользователе, доступная через <see cref="AuthInternalHttpService" />.
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
                                  IEnumerable<string> roles,
                                  IEnumerable<IdentityUserLogin> logins,
                                  List<IdentityUserClaim> claims)
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

            public IEnumerable<string> Roles { get; set; }

            public IEnumerable<IdentityUserLogin> Logins { get; set; }

            public List<IdentityUserClaim> Claims { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity;
using InfinniPlatform.Auth.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Logging;
using InfinniPlatform.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;

using HttpResponse = InfinniPlatform.Http.HttpResponse;

namespace InfinniPlatform.Auth.Services
{
    /// <summary>
    /// Сервис аутентификации пользователей системы.
    /// </summary>
    internal class AuthInternalHttpService : IHttpService
    {
        private const string ApplicationAuthScheme = "Identity.Application";
        private const string ExternalAuthScheme = "Identity.External";
        private readonly IHttpContextProvider _httpContextProvider;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<AppUser> _userManager;

        public AuthInternalHttpService(IHttpContextProvider httpContextProvider,
                                       IUserIdentityProvider userIdentityProvider,
                                       UserEventHandlerInvoker userEventHandlerInvoker,
                                       UserManager<AppUser> userManager,
                                       SignInManager<AppUser> signInManager)
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

            // Методы входа и выхода в систему.
            builder.Post["/SignInInternal"] = SignInInternal;
            builder.Post["/SignInExternal"] = SignInExternal;
            builder.Get["/SignInExternalCallback"] = SignInExternalCallback;
            builder.Post["/SignOut"] = SignOut;

            // Методы работы с учетной записью.
            builder.Post["/GetCurrentUser"] = GetCurrentUser;
            builder.Post["/ChangePassword"] = ChangePassword;

            // Методы для работы с внешними провайдерами аутентификации.
            builder.Post["/GetExternalProviders"] = GetExternalProviders;
            builder.Post["/LinkExternalLogin"] = LinkExternalLogin;
            builder.Get["/LinkExternalLoginCallback"] = LinkExternalLoginCallback;
            builder.Post["/UnlinkExternalLogin"] = UnlinkExternalLogin;
        }


        // Методы входа и выхода в систему.

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        private async Task<object> SignInInternal(IHttpRequest request)
        {
            var signInForm = request.Form;
            string userName = signInForm.UserName;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(userName))
            {
                return CreateErrorResponse(Resources.UserNameCannotBeNullOrWhiteSpace, 400);
            }

            var result = await _signInManager.PasswordSignInAsync(userName, password, remember, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);

                return CreateSuccesResponse(user);
            }
            else
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return CreateErrorResponse(Resources.UserNotFound, 400);
                }

                return CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);
            }
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внешний провайдер.
        /// </summary>
        private async Task<object> SignInExternal(IHttpRequest request)
        {
            var challengeResult = await ChallengeExternalProvider(request, "/Auth/SignInExternalCallback");

            return challengeResult;
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private Task<object> SignInExternalCallback(IHttpRequest request)
        {
            return ChallengeExternalProviderCallback(request, async loginInfo =>
                                                              {
                                                                  var user = await _userManager.GetUserAsync(loginInfo.Principal);

                                                                  if (user == null)
                                                                  {
                                                                      user = CreateUserByLoginInfo(loginInfo);

                                                                      var result = await _userManager.CreateAsync(user);

                                                                      if (result.Succeeded)
                                                                      {
                                                                          await _userManager.AddLoginAsync(user, loginInfo);
                                                                      }
                                                                  }

                                                                  await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);

                                                                  return null;
                                                              });
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private async Task<object> SignOut(IHttpRequest request)
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(request.User);

            // Выход из системы
            await AuthenticationManager.SignOutAsync(ApplicationAuthScheme);
            await AuthenticationManager.SignOutAsync(ExternalAuthScheme);

            var httpResponse = HttpContext.Response;

            return new JsonHttpResponse(new ServiceResult<object> {Success = true}) {StatusCode = httpResponse.StatusCode};
        }


        // Методы входа и выхода в систему.

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

            var changePasswordForm = request.Form;
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


        // Методы для работы с внешними провайдерами аутентификации.

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            var loginProviders = _signInManager.GetExternalAuthenticationSchemes();

            var loginProvidersList = loginProviders.Select(i => new {Type = i.AuthenticationScheme, Name = i.DisplayName})
                                                   .ToArray();

            var httpResponse = CreateSuccesResponse(loginProvidersList);

            return Task.FromResult<object>(httpResponse);
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        private async Task<object> LinkExternalLogin(IHttpRequest request)
        {
            if (!IsAuthenticated())
            {
                return CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var challengeResult = await ChallengeExternalProvider(request, "/Auth/LinkExternalLoginCallback");

            return challengeResult;
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private Task<object> LinkExternalLoginCallback(IHttpRequest request)
        {
            return ChallengeExternalProviderCallback(request, async loginInfo =>
                                                              {
                                                                  var identityUser = await _userManager.GetUserAsync(GetCurrentInternalClaimsPrincipal());

                                                                  // Добавление имени входа пользователя
                                                                  var addLoginTask = await _userManager.AddLoginAsync(identityUser, loginInfo);

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

            var unlinkExternalLoginForm = request.Form;
            string provider = unlinkExternalLoginForm.Provider;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
            }

            var identityUser = await _userManager.GetUserAsync(GetCurrentInternalClaimsPrincipal());
            var identityUserLogin = identityUser.Logins.Find(login => login.LoginProvider == provider);
            // Удаление имени входа пользователя
            var removeLoginTask = await _userManager.RemoveLoginAsync(identityUser, identityUserLogin.LoginProvider, identityUserLogin.ProviderKey);

            if (!removeLoginTask.Succeeded)
            {
                var errorMessage = !removeLoginTask.Succeeded
                                       ? string.Join(Environment.NewLine, removeLoginTask.Errors)
                                       : null;

                return CreateErrorResponse(errorMessage, 400);
            }

            return CreateSuccesResponse<object>(null);
        }


        // Другие расширения.

        /// <summary>
        /// Создает учетную запись пользователя по информации внешнего провайдера.
        /// </summary>
        private static AppUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var email = loginInfo.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            var user = new AppUser
                       {
                           Id = Guid.NewGuid().ToString(),
                           UserName = email.Value,
                           Email = email.Value,
                           EmailConfirmed = !string.IsNullOrEmpty(email.Value)
                       };

            if (loginInfo.Principal.Claims != null)
            {
                user.Claims = loginInfo.Principal.Claims.Select(CreateIdentityUserClaim).ToList();
            }

            return user;
        }


        // CHALLENGE

        /// <summary>
        /// Осуществляет переход на страницу входа внешнего провайдера.
        /// </summary>
        private async Task<IHttpResponse> ChallengeExternalProvider(IHttpRequest request, string callbackPath)
        {
            string loginProvider = request.Form.Provider;

            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
            }

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(loginProvider, callbackPath);

            await AuthenticationManager.ChallengeAsync(loginProvider, properties);

            var httpResponse = HttpContext.Response;
            var response = new HttpResponse(httpResponse.StatusCode, httpResponse.ContentType);

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
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                // Если пользователь прошел аутентификацию через внешний провайдер
                if (externalLoginInfo != null)
                {
                    errorMessage = await callbackAction(externalLoginInfo);
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

        private async Task<AppUser> GetUserInfo()
        {
            var userId = Identity.GetUserId();

            var userInfo = await _userManager.FindByIdAsync(userId);

            return userInfo;
        }

        private static PublicUserInfo BuildPublicUserInfo(AppUser user, IIdentity identity)
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

        private static AppUserClaim CreateIdentityUserClaim(Claim claim)
        {
            return new AppUserClaim
                   {
                       Type = claim.Type,
                       Value = claim.Value
                   };
        }

        private ClaimsIdentity GetCurrentInternalClaimsIdentity()
        {
            return HttpContext.User.Identities.First(identity => identity.AuthenticationType == ApplicationAuthScheme);
        }

        private ClaimsPrincipal GetCurrentInternalClaimsPrincipal()
        {
            return new ClaimsPrincipal(GetCurrentInternalClaimsIdentity());
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
                                  IEnumerable<AppUserLogin> logins,
                                  List<AppUserClaim> claims)
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

            public IEnumerable<AppUserLogin> Logins { get; set; }

            public List<AppUserClaim> Claims { get; set; }
        }
    }
}
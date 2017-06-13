using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using HttpResponse = InfinniPlatform.Http.HttpResponse;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Сервис внешней аутентификации пользователей системы.
    /// </summary>
    internal class AuthExternalHttpService<TUser> : IHttpService where TUser : AppUser, new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<TUser> _signInManager;
        private readonly AuthHttpServiceOptions _authHttpServiceOptions;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<TUser> _userManager;

        public AuthExternalHttpService(IHttpContextAccessor httpContextAccessor,
                                       IUserIdentityProvider userIdentityProvider,
                                       UserEventHandlerInvoker userEventHandlerInvoker,
                                       UserManager<TUser> userManager,
                                       SignInManager<TUser> signInManager,
                                       AuthHttpServiceOptions authHttpServiceOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _userIdentityProvider = userIdentityProvider;
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
            _authHttpServiceOptions = authHttpServiceOptions;
        }

        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private AuthenticationManager AuthenticationManager => HttpContext.Authentication;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";

            builder.Post["/SignInExternal"] = SignInExternal;
            builder.Get["/SignInExternalCallback"] = SignInExternalCallback;

            builder.Post["/GetExternalProviders"] = GetExternalProviders;
            builder.Post["/LinkExternalLogin"] = LinkExternalLogin;
            builder.Get["/LinkExternalLoginCallback"] = LinkExternalLoginCallback;
            builder.Post["/UnlinkExternalLogin"] = UnlinkExternalLogin;
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внешний провайдер.
        /// </summary>
        private async Task<object> SignInExternal(IHttpRequest request)
        {
            return await ChallengeExternalProvider(request, "/Auth/SignInExternalCallback");
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

                                                                  _userEventHandlerInvoker.OnAfterSignIn(Identity);

                                                                  return null;
                                                              });
        }

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        private Task<object> GetExternalProviders(IHttpRequest request)
        {
            var loginProviders = _signInManager.GetExternalAuthenticationSchemes();

            var loginProvidersList = loginProviders.Select(i => new { Type = i.AuthenticationScheme, Name = i.DisplayName })
                                                   .ToArray();

            var httpResponse = Extensions.CreateSuccesResponse(loginProvidersList);

            return Task.FromResult<object>(httpResponse);
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        private async Task<object> LinkExternalLogin(IHttpRequest request)
        {
            if (!Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
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
            if (!Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var unlinkExternalLoginForm = request.Form;
            string provider = unlinkExternalLoginForm.Provider;

            if (string.IsNullOrWhiteSpace(provider))
            {
                return Extensions.CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
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

                return Extensions.CreateErrorResponse(errorMessage, 400);
            }

            return Extensions.CreateSuccesResponse<object>(null);
        }

        /// <summary>
        /// Осуществляет переход на страницу входа внешнего провайдера.
        /// </summary>
        private async Task<IHttpResponse> ChallengeExternalProvider(IHttpRequest request, string callbackPath)
        {
            string loginProvider = request.Form.Provider;

            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Extensions.CreateErrorResponse(Resources.ExternalProviderCannotBeNullOrWhiteSpace, 400);
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
            var successUrl = (string)request.Query.SuccessUrl ?? _authHttpServiceOptions.SuccessUrl;
            var failureUrl = (string)request.Query.FailureUrl ?? _authHttpServiceOptions.FailureUrl;

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


        /// <summary>
        /// Создает учетную запись пользователя по информации внешнего провайдера.
        /// </summary>
        private static TUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var email = loginInfo.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            var user = new TUser
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
            return HttpContext.User.Identities.First(identity => identity.AuthenticationType == Extensions.ApplicationAuthScheme);
        }

        private ClaimsPrincipal GetCurrentInternalClaimsPrincipal()
        {
            return new ClaimsPrincipal(GetCurrentInternalClaimsIdentity());
        }
    }
}
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Auth.HttpService.Controllers
{
    /// <summary>
    /// Сервис внешней аутентификации пользователей системы.
    /// </summary>
    [Route("Auth")]
    public class AuthExternalController<TUser> : Controller where TUser : AppUser, new()
    {
        public AuthExternalController(UserEventHandlerInvoker userEventHandlerInvoker,
                                      UserManager<TUser> userManager,
                                      SignInManager<TUser> signInManager,
                                      AuthHttpServiceOptions authHttpServiceOptions)
        {
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
            _authHttpServiceOptions = authHttpServiceOptions;
        }

        private readonly SignInManager<TUser> _signInManager;
        private readonly AuthHttpServiceOptions _authHttpServiceOptions;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly UserManager<TUser> _userManager;

        /// <summary>
        /// Осуществляет вход пользователя в систему через внешний провайдер.
        /// </summary>
        [HttpPost("SignInExternal")]
        public async Task<object> SignInExternal([FromForm] string provider)
        {
            return await ChallengeExternalProvider(provider, "/Auth/SignInExternalCallback");
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        [HttpGet("SignInExternalCallback")]
        public Task<IActionResult> SignInExternalCallback([FromQuery] string successUrl, [FromQuery] string failureUrl)
        {
            return ChallengeExternalProviderCallback(successUrl ?? _authHttpServiceOptions.SuccessUrl,
                                                     failureUrl ?? _authHttpServiceOptions.FailureUrl,
                                                     async loginInfo =>
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

                                                         _userEventHandlerInvoker.OnAfterSignIn(HttpContext.User.Identity);

                                                         return null;
                                                     });
        }

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        [HttpGet("GetExternalProviders")]
        public async Task<object> GetExternalProviders()
        {
            var loginProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            var loginProvidersList = loginProviders.Select(i => new { Type = i.HandlerType, Name = i.DisplayName })
                                                   .ToArray();

            var httpResponse = Extensions.CreateSuccesResponse(loginProvidersList);

            return httpResponse;
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        [HttpPost("LinkExternalLogin")]
        public async Task<IActionResult> LinkExternalLogin([FromForm] string provider)
        {
            if (!HttpContext.User.Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            var challengeResult = await ChallengeExternalProvider(provider, "/Auth/LinkExternalLoginCallback");

            return challengeResult;
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        [HttpGet("LinkExternalLoginCallback")]
        public Task<IActionResult> LinkExternalLoginCallback([FromQuery] string successUrl, [FromQuery] string failureUrl)
        {
            return ChallengeExternalProviderCallback(successUrl ?? _authHttpServiceOptions.SuccessUrl,
                                                     failureUrl ?? _authHttpServiceOptions.FailureUrl,
                                                     async loginInfo =>
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
        [HttpPost("UnlinkExternalLogin")]
        public async Task<object> UnlinkExternalLogin([FromForm] string provider)
        {
            if (!HttpContext.User.Identity.IsAuthenticated())
            {
                return Extensions.CreateErrorResponse(Resources.RequestIsNotAuthenticated, 401);
            }

            if (string.IsNullOrWhiteSpace(provider))
            {
                return Extensions.CreateErrorResponse(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace, 400);
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
        private async Task<IActionResult> ChallengeExternalProvider(string loginProvider, string callbackPath)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Extensions.CreateErrorResponse(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace, 400);
            }

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(loginProvider, callbackPath);

            await HttpContext.ChallengeAsync(properties);

            var httpResponse = HttpContext.Response;

            // TODO Check.
            return new StatusCodeResult(httpResponse.StatusCode);
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private async Task<IActionResult> ChallengeExternalProviderCallback(string successUrl, string failureUrl, Func<ExternalLoginInfo, Task<string>> callbackAction)
        {
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

            RedirectResult response;

            if (string.IsNullOrEmpty(errorMessage))
            {
                response = new RedirectResult(successUrl);
            }
            else
            {
                response = new RedirectResult(new UrlBuilder(failureUrl).AddQuery("error", errorMessage).ToString());
                Response.Headers.Add("Warning", errorMessage);
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
                           _id = Guid.NewGuid().ToString(),
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
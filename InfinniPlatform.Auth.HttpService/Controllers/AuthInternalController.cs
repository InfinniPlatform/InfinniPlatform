using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Auth.HttpService.Controllers
{
    /// <summary>
    /// Сервис внутренней аутентификации пользователей системы.
    /// </summary>
    [Route("Auth")]
    public class AuthInternalController<TUser> : Controller where TUser : AppUser, new()
    {
        public AuthInternalController(UserEventHandlerInvoker userEventHandlerInvoker,
                                      UserManager<TUser> userManager,
                                      SignInManager<TUser> signInManager,
                                      IUserStore<TUser> userStore)
        {
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _userEmailStore = (IUserEmailStore<TUser>)userStore;
            _userPhoneNumberStoreExtended = (IUserPhoneNumberStoreExtended<TUser>)userStore;
        }

        private readonly SignInManager<TUser> _signInManager;
        private readonly IUserEmailStore<TUser> _userEmailStore;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly UserManager<TUser> _userManager;
        private readonly IUserPhoneNumberStoreExtended<TUser> _userPhoneNumberStoreExtended;
        private readonly IUserStore<TUser> _userStore;


        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        [HttpPost("SignIn")]
        public async Task<object> SignIn([FromForm] string userKey, [FromForm] string password, [FromForm] bool remember)
        {
            if (string.IsNullOrWhiteSpace(userKey))
            {
                return Extensions.CreateErrorResponse(Resources.UserKeyCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userStore.FindByIdAsync(userKey, CancellationToken.None) ??
                          await _userStore.FindByNameAsync(userKey, CancellationToken.None) ??
                          await _userEmailStore.FindByEmailAsync(userKey, CancellationToken.None) ??
                          await _userPhoneNumberStoreExtended.FindByPhoneNumberAsync(userKey, CancellationToken.None);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему по идентификатору через внутренний провайдер.
        /// </summary>
        [HttpPost("SignInById")]
        public async Task<object> SignInById([FromForm] string id, [FromForm] string password, [FromForm] bool remember)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Extensions.CreateErrorResponse(Resources.IdCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userManager.FindByIdAsync(id);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему по имени пользователя через внутренний провайдер.
        /// </summary>
        [HttpPost("SignInByUserName")]
        public async Task<object> SignInByUserName([FromForm] string userName, [FromForm] string password, [FromForm] bool remember)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Extensions.CreateErrorResponse(Resources.UserNameCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userManager.FindByNameAsync(userName);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему по email через внутренний провайдер.
        /// </summary>
        [HttpPost("SignInByUserName")]
        public async Task<object> SignInByEmail([FromForm] string email, [FromForm] string password, [FromForm] bool remember)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Extensions.CreateErrorResponse(Resources.EmailCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userManager.FindByEmailAsync(email);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему по номеру телофона через внутренний провайдер.
        /// </summary>
        [HttpPost("SignInByPhoneNumber")]
        public async Task<object> SignInByPhoneNumber([FromForm] string phoneNumber, [FromForm] string password, [FromForm] bool remember)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Extensions.CreateErrorResponse(Resources.EmailCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userPhoneNumberStoreExtended.FindByPhoneNumberAsync(phoneNumber, CancellationToken.None);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        [HttpPost("SignOut")]
        public async Task<object> SignOut()
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(HttpContext.User.Identity);

            // Выход из системы
            await HttpContext.SignOutAsync(Extensions.ApplicationAuthScheme);
            await HttpContext.SignOutAsync(Extensions.ExternalAuthScheme);

            var httpResponse = HttpContext.Response;

            return new JsonHttpResponse(new ServiceResult<object> { Success = true }) { StatusCode = httpResponse.StatusCode };
        }


        private async Task<object> ProcessPasswordSignIn(TUser appUser, string password, bool remember)
        {
            if (appUser == null)
            {
                return Extensions.CreateErrorResponse(Resources.UserNotFound, 400);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(appUser.UserName, password, remember, false);

            _userEventHandlerInvoker.OnAfterSignIn(HttpContext.User.Identity);

            var serviceResult = signInResult.Succeeded
                                    ? Extensions.CreateSuccesResponse(appUser)
                                    : Extensions.CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);

            return serviceResult;
        }
    }
}
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Сервис внутренней аутентификации пользователей системы.
    /// </summary>
    internal class AuthInternalHttpService<TUser> : IHttpService where TUser : AppUser, new()
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<TUser> _signInManager;
        private readonly IUserEmailStore<TUser> _userEmailStore;
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<TUser> _userManager;
        private readonly IUserPhoneNumberStoreExtended<TUser> _userPhoneNumberStoreExtended;
        private readonly IUserStore<TUser> _userStore;

        public AuthInternalHttpService(IHttpContextAccessor httpContextAccessor,
                                       IUserIdentityProvider userIdentityProvider,
                                       UserEventHandlerInvoker userEventHandlerInvoker,
                                       UserManager<TUser> userManager,
                                       SignInManager<TUser> signInManager,
                                       IUserStore<TUser> userStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _userIdentityProvider = userIdentityProvider;
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _userEmailStore = (IUserEmailStore<TUser>) userStore;
            _userPhoneNumberStoreExtended = (IUserPhoneNumberStoreExtended<TUser>) userStore;
        }


        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();
        private HttpContext HttpContext => _httpContextAccessor.HttpContext;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";

            builder.Post["/SignIn"] = SignIn;
            builder.Post["/SignInById"] = SignInById;
            builder.Post["/SignInByUserName"] = SignInByUserName;
            builder.Post["/SignInByEmail"] = SignInByEmail;
            builder.Post["/SignInByPhoneNumber"] = SignInByPhoneNumber;

            builder.Post["/SignOut"] = SignOut;
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        private async Task<object> SignIn(IHttpRequest request)
        {
            var signInForm = request.Form;
            string userKey = signInForm.UserKey;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

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
        private async Task<object> SignInById(IHttpRequest request)
        {
            var signInForm = request.Form;
            string id = signInForm.Id;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

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
        private async Task<object> SignInByUserName(IHttpRequest request)
        {
            var signInForm = request.Form;
            string userName = signInForm.UserName;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

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
        private async Task<object> SignInByEmail(IHttpRequest request)
        {
            var signInForm = request.Form;
            string email = signInForm.Email;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

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
        private async Task<object> SignInByPhoneNumber(IHttpRequest request)
        {
            var signInForm = request.Form;
            string phoneNumber = signInForm.PhoneNumber;
            string password = signInForm.Password;
            var remember = ((bool?) signInForm.Remember).GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Extensions.CreateErrorResponse(Resources.EmailCannotBeNullOrWhiteSpace, 400);
            }

            var appUser = await _userPhoneNumberStoreExtended.FindByPhoneNumberAsync(phoneNumber, CancellationToken.None);

            return await ProcessPasswordSignIn(appUser, password, remember);
        }

        private async Task<object> ProcessPasswordSignIn(TUser appUser, string password, bool remember)
        {
            if (appUser == null)
            {
                return Extensions.CreateErrorResponse(Resources.UserNotFound, 400);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(appUser.UserName, password, remember, false);

            _userEventHandlerInvoker.OnAfterSignIn(Identity);

            var serviceResult = signInResult.Succeeded
                                    ? Extensions.CreateSuccesResponse(appUser)
                                    : Extensions.CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);

            return serviceResult;
        }


        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private async Task<object> SignOut(IHttpRequest request)
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(request.User);

            // Выход из системы
            await HttpContext.SignOutAsync(Extensions.ApplicationAuthScheme);
            await HttpContext.SignOutAsync(Extensions.ExternalAuthScheme);

            var httpResponse = HttpContext.Response;

            return new JsonHttpResponse(new ServiceResult<object> {Success = true}) {StatusCode = httpResponse.StatusCode};
        }
    }
}
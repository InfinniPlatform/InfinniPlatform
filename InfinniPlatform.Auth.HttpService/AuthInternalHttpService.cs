using System.Security.Principal;
using System.Threading.Tasks;
using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Http;
using InfinniPlatform.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
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
        private readonly UserEventHandlerInvoker _userEventHandlerInvoker;
        private readonly IUserIdentityProvider _userIdentityProvider;
        private readonly UserManager<TUser> _userManager;

        public AuthInternalHttpService(IHttpContextAccessor httpContextAccessor,
                                       IUserIdentityProvider userIdentityProvider,
                                       UserEventHandlerInvoker userEventHandlerInvoker,
                                       UserManager<TUser> userManager,
                                       SignInManager<TUser> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userIdentityProvider = userIdentityProvider;
            _userEventHandlerInvoker = userEventHandlerInvoker;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        private IIdentity Identity => _userIdentityProvider.GetUserIdentity();

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

        private AuthenticationManager AuthenticationManager => HttpContext.Authentication;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Auth";

            builder.Post["/SignInInternal"] = SignInInternal;
            builder.Post["/SignOut"] = SignOut;
        }


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
                return Extensions.CreateErrorResponse(Resources.UserNameCannotBeNullOrWhiteSpace, 400);
            }

            var result = await _signInManager.PasswordSignInAsync(userName, password, remember, false);

            _userEventHandlerInvoker.OnAfterSignIn(Identity);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName);

                return Extensions.CreateSuccesResponse(user);
            }
            else
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return Extensions.CreateErrorResponse(Resources.UserNotFound, 400);
                }

                return Extensions.CreateErrorResponse(Resources.InvalidUsernameOrPassword, 400);
            }
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private async Task<object> SignOut(IHttpRequest request)
        {
            // Вызов обработчика события выхода пользователя
            _userEventHandlerInvoker.OnBeforeSignOut(request.User);

            // Выход из системы
            await AuthenticationManager.SignOutAsync(Extensions.ApplicationAuthScheme);
            await AuthenticationManager.SignOutAsync(Extensions.ExternalAuthScheme);

            var httpResponse = HttpContext.Response;

            return new JsonHttpResponse(new ServiceResult<object> {Success = true}) {StatusCode = httpResponse.StatusCode};
        }
    }
}
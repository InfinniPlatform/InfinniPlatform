﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.InternalIdentity;
using InfinniPlatform.Authentication.Properties;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Owin;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace InfinniPlatform.Authentication.Middleware
{
    /// <summary>
    /// Обработчик HTTP-запросов к подсистеме аутентификации на базе OWIN.
    /// </summary>
    internal sealed class InternalAuthOwinMiddleware : RoutingOwinMiddleware
    {
        private const int TaskTimeout = 60 * 1000;


        public static readonly PathString BasePath
            = new PathString("/Auth");

        // GetCurrentUser

        public static readonly PathString GetCurrentUserPath
            = BasePath + new PathString("/GetCurrentUser");

        // ChangePassword

        public static readonly PathString ChangePasswordPath
            = BasePath + new PathString("/ChangePassword");

        // ChangeProfile

        public static readonly PathString ChangeProfilePath
            = BasePath + new PathString("/ChangeProfile");

        // ChangeActiveRole

        public static readonly PathString ChangeActiveRolePath
            = BasePath + new PathString("/ChangeActiveRole");

        // GetExternalProviders

        public static readonly PathString GetExternalProvidersPath
            = BasePath + new PathString("/GetExternalProviders");

        // SignInInternalPath

        public static readonly PathString SignInInternalPath
            = BasePath + new PathString("/SignInInternal");

        // SignInExternal

        public static readonly PathString SignInExternalPath
            = BasePath + new PathString("/SignInExternal");

        // SignInExternalCallback

        public static readonly PathString SignInExternalCallbackPath
            = BasePath + new PathString("/SignInExternalCallback");

        // LinkExternalLogin

        public static readonly PathString LinkExternalLoginPath
            = BasePath + new PathString("/LinkExternalLogin");

        // LinkExternalLoginCallback

        public static readonly PathString LinkExternalLoginCallbackPath
            = BasePath + new PathString("/LinkExternalLoginCallback");

        // UnlinkExternalLogin

        public static readonly PathString UnlinkExternalLoginPath
            = BasePath + new PathString("/UnlinkExternalLogin");

        // SignOutPath

        public static readonly PathString SignOutPath
            = BasePath + new PathString("/SignOut");

        public InternalAuthOwinMiddleware(OwinMiddleware next)
            : base(next)
        {
            // Методы, связанные с учетной записью пользователя
            RegisterHandler(new RegistrationHandlerBase("GET", GetCurrentUserPath, GetCurrentUser));
            RegisterHandler(new RegistrationHandlerBase("POST", GetCurrentUserPath, GetCurrentUser));
            RegisterHandler(new RegistrationHandlerBase("POST", ChangePasswordPath, ChangePassword));
            RegisterHandler(new RegistrationHandlerBase("POST", ChangeProfilePath, ChangeProfile));
            RegisterHandler(new RegistrationHandlerBase("POST", ChangeActiveRolePath, ChangeActiveRole));

            // Методы, связанные с входом пользователя в систему
            RegisterHandler(new RegistrationHandlerBase("GET", GetExternalProvidersPath, GetExternalProviders));
            RegisterHandler(new RegistrationHandlerBase("POST", GetExternalProvidersPath, GetExternalProviders));
            RegisterHandler(new RegistrationHandlerBase("POST", SignInInternalPath, SignInInternal));
            RegisterHandler(new RegistrationHandlerBase("POST", SignInExternalPath, SignInExternal));

            RegisterHandler(new RegistrationHandlerBase("GET", SignInExternalCallbackPath, SignInExternalCallback));
            RegisterHandler(new RegistrationHandlerBase("POST", LinkExternalLoginPath, LinkExternalLogin));
            RegisterHandler(new RegistrationHandlerBase("GET", LinkExternalLoginCallbackPath, LinkExternalLoginCallback));
            RegisterHandler(new RegistrationHandlerBase("POST", UnlinkExternalLoginPath, UnlinkExternalLogin));
            RegisterHandler(new RegistrationHandlerBase("POST", SignOutPath, SignOut));
        }

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        private static IRequestHandlerResult GetCurrentUser(IOwinContext context)
        {
            var user = FindCurrentUser(context);
            var userInfo = BuildUserInfo(context, user, null);
            return new ValueRequestHandlerResult(userInfo);
        }

        /// <summary>
        /// Изменяет пароль текущего пользователя.
        /// </summary>
        private static IRequestHandlerResult ChangePassword(IOwinContext context)
        {
            dynamic changePasswordForm = ReadRequestBody(context);
            string oldPassword = changePasswordForm.OldPassword;
            string newPassword = changePasswordForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ArgumentException(Resources.NewPasswordCannotBeNullOrWhiteSpace);
            }

            var user = FindCurrentUser(context);
            var userManager = GetUserManager(context);

            var changePasswordTask = string.IsNullOrEmpty(user.PasswordHash)
                ? userManager.AddPasswordAsync(user.Id, newPassword)
                : userManager.ChangePasswordAsync(user.Id, oldPassword, newPassword);

            ThrowIfError(changePasswordTask);

            return EmptyRequestHandlerResult.Instance;
        }

        /// <summary>
        /// Изменяет персональную информацию текущего пользователя.
        /// </summary>
        private static IRequestHandlerResult ChangeProfile(IOwinContext context)
        {
            dynamic profileForm = ReadRequestBody(context);
            string displayName = profileForm.DisplayName;
            string description = profileForm.Description;

            var user = FindCurrentUser(context);
            user.DisplayName = string.IsNullOrWhiteSpace(displayName) ? user.UserName : displayName.Trim();
            user.Description = description;

            var userManager = GetUserManager(context);
            var updateUserTask = userManager.UpdateAsync(user);
            ThrowIfError(updateUserTask);

            var userInfo = BuildUserInfo(context, user, null);
            return new ValueRequestHandlerResult(userInfo);
        }

        /// <summary>
        /// Изменяет активную роль текущего пользователя.
        /// </summary>
        private static IRequestHandlerResult ChangeActiveRole(IOwinContext context)
        {
            dynamic changeActiveRoleForm = ReadRequestBody(context);
            string activeRole = changeActiveRoleForm.ActiveRole;

            if (string.IsNullOrWhiteSpace(activeRole))
            {
                throw new ArgumentException(Resources.ActiveRoleCannotBeNullOrWhiteSpace);
            }

            var userIdentity = GetIdentity(context);
            var currentActiveRole = userIdentity.FindFirstClaim(ApplicationClaimTypes.ActiveRole);

            if (!string.Equals(currentActiveRole, activeRole, StringComparison.OrdinalIgnoreCase))
            {
                var user = FindCurrentUser(context);

                if (user.Roles == null || user.Roles.All(r => !string.Equals(r.Id, activeRole, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException(string.Format(Resources.UserIsNotInRole, activeRole));
                }

                userIdentity.SetClaim(ApplicationClaimTypes.ActiveRole, activeRole);
            }

            return EmptyRequestHandlerResult.Instance;
        }

        /// <summary>
        /// Возвращает список внешних провайдеров входа в систему.
        /// </summary>
        private static IRequestHandlerResult GetExternalProviders(IOwinContext context)
        {
            var authManager = GetAuthManager(context);
            var loginProviders = authManager.GetExternalAuthenticationTypes();
            var loginProvidersList = (loginProviders != null) ? loginProviders.Select(i => new { Type = i.AuthenticationType, Name = i.Caption }).ToArray() : null;
            return new ValueRequestHandlerResult(loginProvidersList);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внутренний провайдер.
        /// </summary>
        private static IRequestHandlerResult SignInInternal(IOwinContext context)
        {
            dynamic signInForm = ReadRequestBody(context);
            string userName = signInForm.UserName;
            string password = signInForm.Password;
            bool? remember = signInForm.Remember;

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException(Resources.UserNameCannotBeNullOrWhiteSpace);
            }

            var userManager = GetUserManager(context);
            var user = userManager.FindAsync(userName, password).Result;

            if (user == null)
            {
                throw new ArgumentException(Resources.InvalidUsernameOrPassword);
            }

            return SignIn(context, user, remember);
        }

        /// <summary>
        /// Осуществляет вход пользователя в систему через внешний провайдер.
        /// </summary>
        private static IRequestHandlerResult SignInExternal(IOwinContext context)
        {
            return ChallengeExternalProvider(context, SignInExternalCallbackPath);
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private static IRequestHandlerResult SignInExternalCallback(IOwinContext context)
        {
            return ChallengeExternalProviderCallback(context, loginInfo =>
                                                              {
                                                                  var userManager = GetUserManager(context);
                                                                  var user = userManager.Find(loginInfo.Login);

                                                                  // Если пользователь не найден в хранилище пользователей системы
                                                                  if (user == null)
                                                                  {
                                                                      // Создание записи о пользователе
                                                                      user = CreateUserByLoginInfo(loginInfo);
                                                                      var createUserTask = userManager.CreateAsync(user);
                                                                      ThrowIfError(createUserTask);

                                                                      // Добавление имени входа пользователя
                                                                      var addLoginTask = userManager.AddLoginAsync(user.Id, loginInfo.Login);
                                                                      ThrowIfError(addLoginTask);
                                                                  }

                                                                  SignIn(context, user, false);
                                                              });
        }

        /// <summary>
        /// Добавляет текущему пользователю имя входа у внешнего провайдера.
        /// </summary>
        private static IRequestHandlerResult LinkExternalLogin(IOwinContext context)
        {
            GetIdentity(context);

            return ChallengeExternalProvider(context, LinkExternalLoginCallbackPath);
        }

        /// <summary>
        /// Принимает подтверждение от внешнего провайдера о входе пользователя в систему.
        /// </summary>
        private static IRequestHandlerResult LinkExternalLoginCallback(IOwinContext context)
        {
            return ChallengeExternalProviderCallback(context, loginInfo =>
                                                              {
                                                                  var userManager = GetUserManager(context);

                                                                  // Определение текущего пользователя
                                                                  var userIdentity = GetIdentity(context);
                                                                  var userId = SecurityExtensions.GetUserId(userIdentity);

                                                                  // Добавление имени входа пользователя
                                                                  var addLoginTask = userManager.AddLoginAsync(userId, loginInfo.Login);
                                                                  ThrowIfError(addLoginTask);
                                                              });
        }

        /// <summary>
        /// Удаляет у текущего пользователя имя входа у внешнего провайдера.
        /// </summary>
        private static IRequestHandlerResult UnlinkExternalLogin(IOwinContext context)
        {
            dynamic unlinkExternalLoginForm = ReadRequestBody(context);
            string provider = unlinkExternalLoginForm.Provider;
            string providerKey = unlinkExternalLoginForm.ProviderKey;

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentException(Resources.ExternalProviderCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(providerKey))
            {
                throw new ArgumentException(Resources.ExternalProviderKeyCannotBeNullOrWhiteSpace);
            }

            var userManager = GetUserManager(context);

            // Определение текущего пользователя
            var userIdentity = GetIdentity(context);
            var userId = SecurityExtensions.GetUserId(userIdentity);

            // Удаление имени входа пользователя
            var removeLoginTask = userManager.RemoveLoginAsync(userId, new UserLoginInfo(provider, providerKey));
            ThrowIfError(removeLoginTask);

            return EmptyRequestHandlerResult.Instance;
        }

        /// <summary>
        /// Осуществляет выход пользователя из системы.
        /// </summary>
        private static IRequestHandlerResult SignOut(IOwinContext context)
        {
            var authManager = GetAuthManager(context);

            // Выход из системы
            authManager.SignOut();

            return EmptyRequestHandlerResult.Instance;
        }

        // Helpers

        /// <summary>
        /// Осуществляет вход указанного пользователя в систему.
        /// </summary>
        private static IRequestHandlerResult SignIn(IOwinContext context, IdentityApplicationUser user, bool? remember)
        {
            var authManager = GetAuthManager(context);
            var userManager = GetUserManager(context);

            // Выход из системы
            authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            // Создание новых учетных данных

            var identity = userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie).Result;

            if (user.DefaultRole != null)
            {
                identity.SetClaim(ApplicationClaimTypes.ActiveRole, user.DefaultRole.Id);
                identity.SetClaim(ApplicationClaimTypes.DefaultRole, user.DefaultRole.Id);
            }

            // Вход в систему с новыми учетными данными
            authManager.SignIn(new AuthenticationProperties { IsPersistent = (remember == true) }, identity);

            var userInfo = BuildUserInfo(context, user, identity);
            return new ValueRequestHandlerResult(userInfo);
        }

        /// <summary>
        /// Возвращает безопасную информацию о пользователе.
        /// </summary>
        private static object BuildUserInfo(IOwinContext context, IdentityApplicationUser user, IIdentity userIdentity)
        {
            // Информация о пользователе включает только безопасные сведения

            if (userIdentity == null)
            {
                userIdentity = GetIdentity(context);
            }

            var claims = GetCurrentUserClaims(user, userIdentity);
            var activeRole = userIdentity.FindFirstClaim(ApplicationClaimTypes.ActiveRole);
            var defaultRole = userIdentity.FindFirstClaim(ApplicationClaimTypes.DefaultRole);

            if (user.DefaultRole != null)
            {
                if (string.IsNullOrEmpty(activeRole))
                {
                    activeRole = user.DefaultRole.Id;
                }

                if (string.IsNullOrEmpty(defaultRole))
                {
                    defaultRole = user.DefaultRole.Id;
                }
            }

            return new
            {
                user.UserName,
                user.DisplayName,
                user.Description,
                ActiveRole = activeRole,
                DefaultRole = defaultRole,
                user.Roles,
                user.Logins,
                Claims = claims
            };
        }

        /// <summary>
        /// Осуществляет переход на страницу входа внешнего провайдера.
        /// </summary>
        private static IRequestHandlerResult ChallengeExternalProvider(IOwinContext context, PathString callbackPath)
        {
            dynamic signInForm = ReadRequestBody(context);
            string provider = signInForm.Provider;
            string successUrl = signInForm.SuccessUrl;
            string failureUrl = signInForm.FailureUrl;

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentException(Resources.ExternalProviderCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(successUrl))
            {
                throw new ArgumentException(Resources.SuccessUrlCannotBeNullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(failureUrl))
            {
                throw new ArgumentException(Resources.FailureUrlCannotBeNullOrWhiteSpace);
            }

            // Адрес возврата для приема подтверждения от внешнего провайдера
            var callbackUri = new UrlBuilder()
                .Relative(callbackPath.Value)
                .AddQuery("SuccessUrl", successUrl)
                .AddQuery("FailureUrl", failureUrl)
                .ToString();

            // Перенаправление пользователя на страницу входа внешнего провайдера
            var authManager = GetAuthManager(context);
            var authProperties = new AuthenticationProperties { RedirectUri = callbackUri };
            authManager.Challenge(authProperties, provider);

            return EmptyRequestHandlerResult.Instance;
        }

        private static IRequestHandlerResult ChallengeExternalProviderCallback(IOwinContext context, Action<ExternalLoginInfo> callbackAction)
        {
            var errorList = new List<string>();
            var successUrl = context.Request.Query.Get("SuccessUrl");
            var failureUrl = context.Request.Query.Get("FailureUrl");

            try
            {
                var authManager = GetAuthManager(context);
                var loginInfo = authManager.GetExternalLoginInfo();

                // Если пользователь прошел аутентификацию через внешний провайдер
                if (loginInfo != null)
                {
                    callbackAction(loginInfo);
                }
                else
                {
                    errorList.Add(Resources.UnsuccessfulSignInWithExternalProvider);
                }
            }
            catch (AggregateException error)
            {
                if (error.InnerExceptions != null)
                {
                    foreach (var innerException in error.InnerExceptions)
                    {
                        errorList.Add(innerException.Message);
                    }
                }
                else
                {
                    errorList.Add(error.Message);
                }
            }
            catch (Exception error)
            {
                errorList.Add(error.Message);
            }

            // Перенаправление пользователя на страницу приложения

            var redirectUrl = successUrl;
            var response = context.Response;

            if (errorList.Count > 0)
            {
                redirectUrl = new UrlBuilder(failureUrl).AddQuery("error", string.Join(Environment.NewLine, errorList)).ToString();
                response.Headers.AppendValues("Warning", errorList.ToArray());
            }

            response.Redirect(redirectUrl);

            return EmptyRequestHandlerResult.Instance;
        }

        /// <summary>
        /// Возвращает сведения о текущем пользователе системы.
        /// </summary>
        private static IdentityApplicationUser FindCurrentUser(IOwinContext context)
        {
            var userIdentity = GetIdentity(context);
            var userId = SecurityExtensions.GetUserId(userIdentity);

            if (userId != null)
            {
                var userManager = GetUserManager(context);
                var user = userManager.FindById(userId);

                if (user != null)
                {
                    return user;
                }
            }

            throw new InvalidOperationException(Resources.UserNotFound);
        }

        private static IdentityApplicationUser CreateUserByLoginInfo(ExternalLoginInfo loginInfo)
        {
            var userName = loginInfo.DefaultUserName;

            if (loginInfo.Login != null)
            {
                userName = string.Format("{0}{1}", loginInfo.Login.LoginProvider, loginInfo.Login.ProviderKey).Replace(" ", "");
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
                user.Claims = loginInfo.ExternalIdentity.Claims.Select(CreateApplicationUserClaim);
            }

            return user;
        }

        private static IEnumerable<ApplicationUserClaim> GetCurrentUserClaims(IdentityApplicationUser user, IIdentity userIdentity)
        {
            var result = new List<ApplicationUserClaim>();

            if (user != null && user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    if (claim.Type != null && claim.Value != null)
                    {
                        result.Add(claim);
                    }
                }
            }

            var identity = userIdentity as ClaimsIdentity;

            if (identity != null && identity.Claims != null)
            {
                foreach (var claim in identity.Claims)
                {
                    if (claim.Type != null && !result.Exists(c => string.Equals(c.Type.Id, claim.Type, StringComparison.OrdinalIgnoreCase) && string.Equals(c.Value, claim.Value, StringComparison.Ordinal)))
                    {
                        result.Add(CreateApplicationUserClaim(claim));
                    }
                }
            }

            return result;
        }

        private static ApplicationUserClaim CreateApplicationUserClaim(Claim claim)
        {
            return new ApplicationUserClaim
            {
                Type = new ForeignKey { Id = claim.Type },
                Value = claim.Value
            };
        }

        private static IIdentity GetIdentity(IOwinContext context)
        {
            var principal = context.Request.User;

            if (principal == null)
            {
                throw new InvalidOperationException(Resources.RequestIsNotAuthenticated);
            }

            return principal.Identity;
        }

        private static IAuthenticationManager GetAuthManager(IOwinContext context)
        {
            var authManager = context.Authentication;

            if (authManager == null)
            {
                throw new InvalidOperationException(Resources.AuthenticationManagerNotFound);
            }

            return authManager;
        }

        private static UserManager<IdentityApplicationUser> GetUserManager(IOwinContext context)
        {
            var userManager = context.GetUserManager<UserManager<IdentityApplicationUser>>();

            if (userManager == null)
            {
                throw new InvalidOperationException(Resources.UserManagerNotFound);
            }

            return userManager;
        }

        private static void ThrowIfError(Task<IdentityResult> task)
        {
            if (!task.Wait(TaskTimeout))
            {
                throw new InvalidOperationException(Resources.OperationHasCancelledByTimeout);
            }

            if (!task.Result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, task.Result.Errors));
            }
        }
    }
}
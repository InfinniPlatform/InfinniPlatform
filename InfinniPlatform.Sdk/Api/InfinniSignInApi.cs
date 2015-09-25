using System;
using System.Net;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для аутентификации пользователей
    /// </summary>
    public sealed class InfinniSignInApi : BaseApi, ISignInApi
    {

        public InfinniSignInApi(string server, string port, string route)
            : base(server, port, route)
        {
        }

        /// <summary>
        ///   Зарегистрироваться с использованием внутреннего хранилища пользователей
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="remember">Запомнить пользователя</param>
        /// <returns>Результат попытки регистрации</returns>
        public dynamic SignInInternal(string userName, string password, bool remember)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var requestBody = new
            {
                UserName = userName,
                Password = password,
                Remember = remember
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlSignIn(),requestBody);

            var cookieContainer = ProcessAsObjectResult(response,
                            string.Format(Resources.UnableToSignInUser, response.GetErrorContent()));   

            CreateCookieContainer(cookieContainer);

            return cookieContainer;
        }

        /// <summary>
        ///   Сменить пароль пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="oldPassword">Старый пароль пользователя</param>
        /// <param name="newPassword">Новый пароль пользователя</param>
        /// <returns>Признак успешной смены пароля пользователя</returns>
        public dynamic ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var requestBody = new
            {
                UserName = userName,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingChangePassword(), requestBody);

            return ProcessAsObjectResult(response,
                            string.Format(Resources.UnableToChangePasswordUser, response.GetErrorContent()));   

        }

        /// <summary>
        ///   Выйти из системы
        /// </summary>
        /// <returns>Признак успешного выхода из системы</returns>
        public dynamic SignOut()
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlSignOut());

            return ProcessAsObjectResult(response,
                            string.Format(Resources.UnableToSignOutUser, response.GetErrorContent())); 

        }

        private void CreateCookieContainer(dynamic result)
        {
            if (result != null && result.ResponseCookies != null && result.UserInfo != null)
            {
                CookieContainer = new CookieContainer();

                foreach (var cookie in result.ResponseCookies)
                {
                    var responseCookie = new Cookie()
                    {
                        Name = cookie.Name.ToString(),
                        Value = cookie.Value.ToString(),
						//TODO В куки попадал localhost, вместо реального адреса сервера.
						Domain = Server,
                        Expires = Convert.ToDateTime(cookie.Expires),
                        Path = cookie.Path.ToString(),
                        Discard = Convert.ToBoolean(cookie.Discard)
                    };
                    CookieContainer.Add(responseCookie);
                }
            }
        }


    }
}

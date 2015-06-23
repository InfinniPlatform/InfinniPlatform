using System;
using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Dynamic;
using RestSharp;

namespace InfinniPlatform.Api.RestApi.Auth
{
    /// <summary>
    ///     API для входа и выхода из системы
    ///     Вызывается только с клиента
    /// </summary>
    public sealed class SignInApi
    {
        private readonly string _version;

        public SignInApi(string version = null)
        {
            _version = version;
        }

        public static CookieContainer CookieContainer { get; private set; }

        /// <summary>
        ///     Зарегистрироваться в системе
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="remember">Запомнить пароль</param>
        public dynamic SignInInternal(string userName, string password, bool remember)
        {
            //TODO необходима доработка регистрации пользователя: должен возвращать Cookie с результатом
            var signInInternalUrl = ControllerRoutingFactory.Instance.GetCustomRouting("Auth/SignInInternal");


            var restClient = new RestClient(signInInternalUrl);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };
            request.AddParameter("UserName", userName)
                .AddParameter("Password", password);

            var restResponse = restClient.Post(request);
            if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException(string.Format("Fail in process of sign in user: {0} : {1} : ({2})",
                    restResponse.ErrorException, restResponse.ErrorMessage, restResponse.Content));
            }

            CookieContainer = new CookieContainer();

            var responseCookies = new List<dynamic>();

            foreach (var cookie in restResponse.Cookies)
            {
                var responseCookie = new Cookie
                {
                    Name = cookie.Name,
                    Value = cookie.Value,
                    Domain = cookie.Domain,
                    Expires = cookie.Expires,
                    Path = cookie.Path,
                    Discard = cookie.Discard
                };
                CookieContainer.Add(responseCookie);

                responseCookies.Add(responseCookie);
            }

            dynamic result = new DynamicWrapper();

            //гребаный JsonObjectSerializer добавляет служебный символ в начале при сериализации
            result.UserInfo = restResponse.Content.Substring(1).ToDynamic();

            result.ResponseCookies = responseCookies.ToEnumerable();

            result.IsValid = true;

            return result;
        }

        /// <summary>
        ///     Выйти из системы
        /// </summary>
        public dynamic SignOutInternal()
        {
            //----------------------------------------------------
            var signOutInternalUrl = ControllerRoutingFactory.Instance.GetCustomRouting("Auth/SignOut");

            var restClient = new RestClient(signOutInternalUrl);

            var request = new RestRequest
            {
                RequestFormat = DataFormat.Json
            }.AddBody("");


            var restResponse = restClient.Post(request);
            if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException(string.Format("Fail in process of sign out user: {0}", restResponse.Content));
            }
            //----------------------------------------------------
            //необходима доработка разлогинивания пользователей (при текущей реализации не разлогинивает пользователя на клиенте)
            CookieContainer = null;

            dynamic result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = "User sign out successfully";

            return result;
        }

        /// <summary>
        ///     Добавить пользователя во внутреннее хранилище
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="oldPassword">старый пароль</param>
        /// <param name="newPassword">старый пароль</param>
        public dynamic ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "authorization", "changepassword", null, new
            {
                UserName = userName,
                OldPassword = oldPassword,
                NewPassword = newPassword
            }, _version).ToDynamic();
        }
    }
}
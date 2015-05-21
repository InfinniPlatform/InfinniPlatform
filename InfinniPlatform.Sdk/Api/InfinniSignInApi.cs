using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   API для аутентификации пользователей
    /// </summary>
    public sealed class InfinniSignInApi
    {
        private readonly string _server;
        private readonly string _port;
        private readonly string _version;
        private CookieContainer _cookieContainer;
        private readonly RouteBuilder _routeBuilder;

        public InfinniSignInApi(string server, string port, string version)
        {
            _server = server;
            _port = port;
            _version = version;
            _routeBuilder = new RouteBuilder(_server, _port);
        }

        /// <summary>
        ///   Контейнер аутентификационной информации
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return _cookieContainer; }
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var requestBody = new
            {
                UserName = userName,
                Password = password,
                Remember = remember
            };

            var response = restQueryExecutor.QueryPost(_routeBuilder.BuildRestRoutingUrlSignIn(_version),requestBody);

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        dynamic result = JObject.Parse(response.Content.Remove(0, 1));
                        CreateCookieContainer(result);
                        return result;
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToSignUser, response.GetErrorContent()));
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
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var requestBody = new
            {
                UserName = userName,
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var response = restQueryExecutor.QueryPost(_routeBuilder.BuildRestRoutingChangePassword(_version), requestBody);

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FaiilToChangePasswordUser, response.GetErrorContent()));
        }

        /// <summary>
        ///   Выйти из системы
        /// </summary>
        /// <returns>Признак успешного выхода из системы</returns>
        public dynamic SignOut()
        {
            var restQueryExecutor = new RequestExecutor(_cookieContainer);

            var response = restQueryExecutor.QueryPost(_routeBuilder.BuildRestRoutingUrlSignOut(_version));

            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                        return JObject.Parse(response.Content.Remove(0, 1));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException(Resources.ResultIsNotOfObjectType);
                }
            }
            throw new ArgumentException(string.Format(Resources.FailToSignOutUser, response.GetErrorContent()));
        }

        private void CreateCookieContainer(dynamic result)
        {
            if (result != null && result.ResponseCookies != null && result.UserInfo != null)
            {
                _cookieContainer = new CookieContainer();

                foreach (var cookie in result.ResponseCookies)
                {
                    var responseCookie = new Cookie()
                    {
                        Name = cookie.Name.ToString(),
                        Value = cookie.Value.ToString(),
                        Domain = cookie.Domain.ToString(),
                        Expires = Convert.ToDateTime(cookie.Expires),
                        Path = cookie.Path.ToString(),
                        Discard = Convert.ToBoolean(cookie.Discard)
                    };
                    _cookieContainer.Add(responseCookie);
                }
            }
        }
    }
}

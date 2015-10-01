using System;
using System.Collections.Generic;
using System.Net;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    ///   Базовый класс для всех API системы
    /// </summary>
    public class BaseApi
    {
        private readonly string _server;
        private readonly string _port;
        private readonly RouteBuilder _routeBuilder;
        private readonly string _route;

        public BaseApi(string server, string port, string route)
        {
            _server = server;
            _port = port;
            _route = route;
            _routeBuilder = new RouteBuilder(server, port, route);
        }

        /// <summary>
        ///   Контейнер аутентификационных данных
        /// </summary>
        public CookieContainer  CookieContainer { get; set; }

        /// <summary>
        ///   Обработать результат выполнения запроса как объект
        /// </summary>
        /// <param name="response">Результат выполнения запроса</param>
        /// <param name="exceptionMessage">Сообщение в случае ошибки выполнения запроса</param>
        protected dynamic ProcessAsObjectResult(RestQueryResponse response, string exceptionMessage)
        {
            if (!string.IsNullOrEmpty(response.Content))
            {
                //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
                return ProcessRequestResult(response, () => response.Content.Remove(0, 1).ToDynamic(),
                    () => Resources.ResultIsNotOfObjectType, () => exceptionMessage);
            }
            return null;
        }

        /// <summary>
        ///   Обработать результат выполнения запроса
        /// </summary>
        private dynamic ProcessRequestResult(RestQueryResponse response, Func<dynamic> processResultMethod, 
            Func<string> getRequestFormatExceptionMessage, Func<string> getRequestExceptionMessage)
        {
            if (response.IsAllOk)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {                        
                        return processResultMethod();
                    }
                }
                catch
                {
                    throw new ApplicationException(getRequestFormatExceptionMessage());
                }
            }
            throw new ApplicationException(getRequestExceptionMessage());
        }


        /// <summary>
        ///   Конструктор роутинга запросов API
        /// </summary>
        protected RouteBuilder RouteBuilder
        {
            get { return _routeBuilder; }
        }

        /// <summary>
        ///   Адрес сервера
        /// </summary>
        protected string Server
        {
            get { return _server; }
        }

        /// <summary>
        ///  Порт сервера
        /// </summary>
        protected string Port
        {
            get { return _port; }
        }

        protected string Route
        {
            get { return _route; }
        }
    }
}

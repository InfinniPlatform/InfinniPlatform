using System;
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
    ///   Базовый класс для всех API системы
    /// </summary>
    public class BaseApi
    {
        private readonly string _server;
        private readonly string _port;
        private readonly string _version;
        private readonly RouteBuilder _routeBuilder;

        public BaseApi(string server, string port, string version)
        {
            _server = server;
            _port = port;
            _version = version;
            _routeBuilder = new RouteBuilder(server, port);
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
            //гребаный JsonObjectSerializer вставляет служебный символ в начало строки
            return ProcessRequestResult(response, () => JObject.Parse(response.Content.Remove(0, 1)), () => Resources.ResultIsNotOfObjectType, () => exceptionMessage);
        }


        /// <summary>
        ///  Обработать результат выполнения запроса как массив
        /// </summary>
        /// <param name="response">Результат выполнения запроса</param>
        /// <param name="exceptionMessage">Сообщение в случае ошибки выполнения запроса</param>
        /// <returns>Список объектов результата</returns>
        protected IEnumerable<dynamic> ProcessAsArrayResult(RestQueryResponse response, string exceptionMessage)
        {
            return ProcessRequestResult(response, () => JArray.Parse(response.Content.Remove(0, 1)), () => Resources.ResultIsNotOfArrayType, () => exceptionMessage);   
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
        ///  Версия приложения
        /// </summary>
        public string Version
        {
            get { return _version; }
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
    }
}

using System;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    /// Базовый класс для всех API системы
    /// </summary>
    public class BaseApi
    {
        public BaseApi(string server, string port, string route)
        {
            Server = server;
            Port = port;
            Route = route;
            RouteBuilder = new RouteBuilder(server, port, route);
        }

        /// <summary>
        /// Конструктор роутинга запросов API
        /// </summary>
        protected RouteBuilder RouteBuilder { get; }

        /// <summary>
        /// Адрес сервера
        /// </summary>
        protected string Server { get; }

        /// <summary>
        /// Порт сервера
        /// </summary>
        protected string Port { get; }

        protected string Route { get; }

        protected RequestExecutor RequestExecutor => RequestExecutor.Instance;

        /// <summary>
        /// Обработать результат выполнения запроса как объект
        /// </summary>
        /// <param name="response">Результат выполнения запроса</param>
        /// <param name="exceptionMessage">Сообщение в случае ошибки выполнения запроса</param>
        protected dynamic ProcessAsObjectResult(string response, string exceptionMessage)
        {
            if (!string.IsNullOrEmpty(response))
            {
                return ProcessRequestResult(response, () => response.ToDynamic(), () => Resources.ResultIsNotOfObjectType, () => exceptionMessage);
            }
            return null;
        }

        /// <summary>
        /// Обработать результат выполнения запроса
        /// </summary>
        private dynamic ProcessRequestResult(string response, Func<dynamic> processResultMethod,
                                             Func<string> getRequestFormatExceptionMessage, Func<string> getRequestExceptionMessage)
        {
            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    return processResultMethod();
                }
            }
            catch
            {
                throw new ApplicationException(getRequestFormatExceptionMessage());
            }

            throw new ApplicationException(getRequestExceptionMessage());
        }
    }
}
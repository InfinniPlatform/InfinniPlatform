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
        public BaseApi(string server, int port)
        {
            Server = server;
            Port = port;
            Route = "0";
            RouteBuilder = new RouteBuilder(server, port, "0");
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
        protected int Port { get; }

        protected string Route { get; }

        protected RequestExecutor RequestExecutor => RequestExecutor.Instance;

        /// <summary>
        /// Обработать результат выполнения запроса как объект
        /// </summary>
        /// <param name="response">Результат выполнения запроса</param>
        /// <param name="exceptionMessage">Сообщение в случае ошибки выполнения запроса</param>
        protected static dynamic ProcessAsObjectResult(RestQueryResponse response, string exceptionMessage)
        {
            if (!string.IsNullOrEmpty(response.Content))
            {
                return ProcessRequestResult(response, () => response.Content.ToDynamic(), () => Resources.ResultIsNotOfObjectType, () => exceptionMessage);
            }

            return null;
        }

        /// <summary>
        /// Обработать результат выполнения запроса
        /// </summary>
        private static dynamic ProcessRequestResult(RestQueryResponse response, Func<dynamic> processResultMethod, Func<string> getRequestFormatExceptionMessage, Func<string> getRequestExceptionMessage)
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
                    throw new InvalidOperationException(getRequestFormatExceptionMessage());
                }
            }

            throw new InvalidOperationException(getRequestExceptionMessage());
        }
    }
}
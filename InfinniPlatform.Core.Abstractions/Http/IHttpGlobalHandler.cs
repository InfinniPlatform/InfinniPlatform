using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Глобальный обработчик запросов.
    /// </summary>
    public interface IHttpGlobalHandler
    {
        /// <summary>
        /// Предобработчик запросов.
        /// </summary>
        Func<HttpRequest, Task<object>> OnBefore { get; set; }

        /// <summary>
        /// Постобработчик запросов.
        /// </summary>
        Func<HttpRequest, object, Task<object>> OnAfter { get; set; }

        /// <summary>
        /// Обработчик исключений.
        /// </summary>
        Func<HttpRequest, Exception, Task<object>> OnError { get; set; }

        /// <summary>
        /// Конвертер результата.
        /// </summary>
        Func<object, HttpResponse> ResultConverter { get; set; }
    }
}
using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Глобальный обработчик запросов.
    /// </summary>
    public interface IHttpGlobalHandler
    {
        /// <summary>
        /// Предобработчик запросов.
        /// </summary>
        Func<IHttpRequest, Task<object>> OnBefore { get; set; }

        /// <summary>
        /// Постобработчик запросов.
        /// </summary>
        Func<IHttpRequest, object, Task<object>> OnAfter { get; set; }

        /// <summary>
        /// Обработчик исключений.
        /// </summary>
        Func<IHttpRequest, Exception, Task<object>> OnError { get; set; }

        /// <summary>
        /// Конвертер результата.
        /// </summary>
        Func<object, IHttpResponse> ResultConverter { get; set; }
    }
}
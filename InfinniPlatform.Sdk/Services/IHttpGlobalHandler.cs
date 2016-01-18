using System;

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
        Func<IHttpRequest, object> OnBefore { get; set; }

        /// <summary>
        /// Постобработчик запросов.
        /// </summary>
        Func<IHttpRequest, object, object> OnAfter { get; set; }

        /// <summary>
        /// Обработчик исключений.
        /// </summary>
        Func<IHttpRequest, Exception, object> OnError { get; set; }

        /// <summary>
        /// Конвертер результата.
        /// </summary>
        Func<object, IHttpResponse> ResultConverter { get; set; }
    }
}
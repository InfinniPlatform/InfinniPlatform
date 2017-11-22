using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Регистратор обработчиков запросов сервиса.
    /// </summary>
    public interface IHttpServiceBuilder
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
        Func<object, IActionResult> ResultConverter { get; set; }
    }
}
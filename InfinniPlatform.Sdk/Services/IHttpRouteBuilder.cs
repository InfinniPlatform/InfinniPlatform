using System;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Регистратор правил маршрутизации запросов.
    /// </summary>
    public interface IHttpRouteBuilder
    {
        /// <summary>
        /// Устанавливает обработчик запросов.
        /// </summary>
        /// <param name="path">Путь запроса.</param>
        /// <returns>Обработчик запроса.</returns>
        Func<IHttpRequest, IHttpResponse> this[string path] { set; }
    }
}
using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Регистратор правил маршрутизации запросов сервиса.
    /// </summary>
    public interface IHttpServiceRouteBuilder
    {
        /// <summary>
        /// Правила маршрутизации запросов.
        /// </summary>
        IEnumerable<IHttpServiceRoute> Routes { get; }

            /// <summary>
        /// Устанавливает обработчик запросов.
        /// </summary>
        /// <param name="path">Путь запроса.</param>
        /// <returns>Обработчик запроса.</returns>
        Func<IHttpRequest, object> this[string path] { set; }
    }
}
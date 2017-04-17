using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Core.Abstractions.Http
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
        Func<IHttpRequest, Task<object>> this[string path] { set; }
    }
}
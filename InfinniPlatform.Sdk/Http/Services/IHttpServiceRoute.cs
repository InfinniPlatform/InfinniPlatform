﻿using System;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Правило маршрутизации запросов.
    /// </summary>
    public interface IHttpServiceRoute
    {
        /// <summary>
        /// Путь запроса.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Обработчик запросов.
        /// </summary>
        Func<IHttpRequest, Task<object>> Action { get; }
    }
}
﻿using System.Collections.Generic;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Заголовок запроса.
    /// </summary>
    public interface IHttpRequestHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        /// <summary>
        /// Список ключей заголовка.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Список значений заголовка.
        /// </summary>
        IEnumerable<IEnumerable<string>> Values { get; }

        /// <summary>
        /// Возвращает значение заголовка по ключу.
        /// </summary>
        IEnumerable<string> this[string key] { get; }


        /// <summary>
        /// Имя агента пользователя.
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Тип содержимого тела запроса.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Размер тела запроса.
        /// </summary>
        long ContentLength { get; }
    }
}
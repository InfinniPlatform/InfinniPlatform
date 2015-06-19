using System;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Соответствие роутинга для обработчиков
    /// </summary>
    public sealed class HandlerRouting
    {
        /// <summary>
        ///     Способ получения роутинга из контекста запроса
        /// </summary>
        public Func<IOwinContext, PathStringProvider> ContextRouting { get; set; }

        /// <summary>
        ///     Метод (POST/GET/DELETE) запроса
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///     Обработчик запроса
        /// </summary>
        public Func<IOwinContext, IRequestHandlerResult> Handler { get; set; }
    }
}
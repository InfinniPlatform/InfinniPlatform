using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///   Соответствие роутинга для обработчиков
    /// </summary>
    public sealed class HandlerRouting
    {
        /// <summary>
        ///   Способ получения роутинга из контекста запроса
        /// </summary>
        public Func<IOwinContext, PathString> ContextRouting { get; set; }

        /// <summary>
        ///   Метод (POST/GET/DELETE) запроса
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///   Обработчик запроса
        /// </summary>
        public Func<IOwinContext, IRequestHandlerResult> Handler { get; set; }

    }


}

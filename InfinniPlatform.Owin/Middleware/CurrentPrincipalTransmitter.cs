using System;
using System.Threading;
using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Вынужденно добавленный класс, вследствие необходимости передачи
    ///     контекста в Thread.CurrentThread
    /// </summary>
    public static class CurrentPrincipalTransmitter
    {
        /// <summary>
        ///     Заполнить контекст авторизации пользователя
        /// </summary>
        /// <param name="handler">Обработчик запроса</param>
        /// <returns>Обработчик запроса</returns>
        public static Func<IOwinContext, IRequestHandlerResult> CreateAuthenticatedRequest(
            this Func<IOwinContext, IRequestHandlerResult> handler)
        {
            return context =>
            {
                Thread.CurrentPrincipal = context.Request.User;
                return handler(context);
            };
        }
    }
}
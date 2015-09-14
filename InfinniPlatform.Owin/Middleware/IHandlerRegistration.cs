using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    ///     Регистрация обработчика запроса
    /// </summary>
    public interface IHandlerRegistration
    {
        /// <summary>
        ///     Уровень приоритета при выборе соответствующего обработчика (в случае регистрации нескольких обработчиков для одного
        ///     роутинга)
        /// </summary>
        Priority Priority { get; }

        /// <summary>
        ///     Метод, соответствующий запросу
        /// </summary>
        string Method { get; }

        /// <summary>
        ///     Признак обработки указанного роутинга запроса
        /// </summary>
        /// <param name="context">Контекст обработки запроса</param>
        /// <param name="requestPath">Роутинг запроса</param>
        /// <returns>Признак разрешения обработки запроса</returns>
        bool CanProcessRequest(IOwinContext context, string requestPath);

        /// <summary>
        ///     Выполнить обработчик запроса
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        IRequestHandlerResult Execute(IOwinContext context);
    }
}
using System;
using System.Linq;
using System.Threading;
using InfinniPlatform.Owin.Middleware;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    ///   Регистрация обработчика запроса 
    /// </summary>
    public abstract class HandlerRegistration : IHandlerRegistration
    {
        private readonly IRouteFormatter _routeFormatter;
        private readonly RequestPathConstructor _constructor;
        private readonly Priority _priority;
        private readonly string _method;

        protected HandlerRegistration(IRouteFormatter routeFormatter, RequestPathConstructor constructor, Priority priority, string method)
        {
            _routeFormatter = routeFormatter;
            _constructor = constructor;
            _priority = priority;
            _method = method;
        }


        /// <summary>
        ///Получить провайдер роутинга запроса для обработчика
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <returns>Провайдер роутинга запросов</returns>
        protected abstract PathStringProvider GetPath(IOwinContext context);

        ///  <summary>
        /// Выполнить обработчик запроса 
        ///  </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        protected abstract IRequestHandlerResult ExecuteHandler(IOwinContext context);

        /// <summary>
        /// Признак обработки указанного роутинга запроса
        /// </summary>
        /// <param name="context">Контекст обработки запроса</param>
        /// <param name="requestPath">Роутинг запроса</param>
        /// <returns>Признак разрешения обработки запроса</returns>
        public bool CanProcessRequest(IOwinContext context, string requestPath)
        {
            return _routeFormatter.FormatRoutePath(context, GetPath(context).PathString).NormalizePath().ToLowerInvariant() ==
                requestPath.ToLowerInvariant();
        }

        /// <summary>
        ///Выполнить обработчик запроса 
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IRequestHandlerResult Execute(IOwinContext context)
        {
            Thread.CurrentPrincipal = context.Request.User;
            return ExecuteHandler(context);
        }

        /// <summary>
        ///   Уровень приоритета при выборе соответствующего обработчика (в случае регистрации нескольких обработчиков для одного роутинга)
        /// </summary>
        public Priority Priority
        {
            get { return _priority; }
        }

        /// <summary>
        ///   Метод, соответствующий запросу
        /// </summary>
        public string Method
        {
            get { return _method; }
        }

        /// <summary>
        ///   Конструктор роутинга запросов
        /// </summary>
        protected RequestPathConstructor PathConstructor
        {
            get { return _constructor; }
        }

        /// <summary>
        ///  Провайдер параметров роутинга
        /// </summary>
        protected IRouteFormatter RouteFormatter
        {
            get { return _routeFormatter; }
        }
    }

    public static class UserAuthHandlerRegistrationExtensions
    {
        public static string ReplaceFormat(this string processingString, string oldString, string newString)
        {
            return processingString.Replace(oldString, newString);
        }

        public static string NormalizePath(this PathString path)
        {
            if (path.HasValue)
            {
                return path.Value.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).First().TrimEnd('/').ToLower();
            }

            return string.Empty;
        }
    }
}

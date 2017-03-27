using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Logging;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Обработчик HTTP-запросов для обработки ошибок выполнения запросов.
    /// </summary>
    [LoggerName("OWIN")]
    internal class ErrorHandlingOwinMiddleware : OwinMiddleware
    {
        private static readonly Task EmptyTask = Task.FromResult<object>(null);


        public ErrorHandlingOwinMiddleware(RequestDelegate next, ILog log, IPerformanceLog performanceLog) : base(next)
        {
            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        public override Task Invoke(HttpContext context)
        {
            var start = DateTime.Now;

            try
            {
                var requestId = context.TraceIdentifier;

                // Установка контекста логирования ошибок текущего потока.
                _log.SetRequestId(requestId);

                return Next.Invoke(context)
                           .ContinueWith(task =>
                                         {
                                             // Повторная установка контекста логирования ошибок текущего потока,
                                             // так как ContinueWith работает не в потоке обработки запроса.
                                             _log.SetRequestId(requestId);
                                             _log.SetUserId(context.User?.Identity);

                                             if (task.IsFaulted)
                                             {
                                                 LogException(task.Exception);
                                             }

                                             LogPerformance($"{context.Request.Method}::{context.Request.Path}", start, task.Exception);

                                             return EmptyTask;
                                         });
            }
            catch (Exception exception)
            {
                LogException(exception);
                LogPerformance("Invoke", start, exception);

                return EmptyTask;
            }
        }


        private void LogException(Exception exception)
        {
            _log.Error(Resources.UnhandledExceptionOwinMiddleware, exception);
        }

        private void LogPerformance(string method, DateTime start, Exception exception)
        {
            _performanceLog.Log(method, start, exception);
        }
    }
}
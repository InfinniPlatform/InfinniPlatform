using System;
using System.Threading.Tasks;

using InfinniPlatform.Owin.Properties;
using InfinniPlatform.Sdk.Logging;

using Microsoft.Owin;

namespace InfinniPlatform.Owin.Middleware
{
    /// <summary>
    /// Обработчик HTTP-запросов для обработки ошибок выполнения запросов.
    /// </summary>
    [LoggerName("OWIN")]
    internal sealed class ErrorHandlingOwinMiddleware : OwinMiddleware
    {
        private static readonly Task EmptyTask = Task.FromResult<object>(null);

        public ErrorHandlingOwinMiddleware(OwinMiddleware next, ILog log, IPerformanceLog performanceLog) : base(next)
        {
            _log = log;
            _performanceLog = performanceLog;
        }

        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;

        public override Task Invoke(IOwinContext context)
        {
            var start = DateTime.Now;

            try
            {
                var requestId = context.Environment?["owin.RequestId"];

                // Установка контекста логирования ошибок текущего потока.
                _log.SetRequestId(requestId);

                return Next.Invoke(context)
                           .ContinueWith(task =>
                                         {
                                             // Повторная установка контекста логирования ошибок текущего потока,
                                             // так как ContinueWith работает не в потоке обработки запроса.
                                             _log.SetRequestId(requestId);
                                             _log.SetUserId(context.Request.User?.Identity);

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
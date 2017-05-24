using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Properties;

using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки ошибок выполнения запросов.
    /// </summary>
    [LoggerName("OWIN")]
    internal class ErrorHandlingMiddleware : IErrorHandlingMiddleware
    {
        public ErrorHandlingMiddleware(ILog log, IPerformanceLog performanceLog)
        {
            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        public void Configure(IApplicationBuilder app)
        {
            app.Use((httpContext, next) =>
                    {
                        var start = DateTime.Now;

                        try
                        {
                            // Установка контекста логирования ошибок текущего потока.
                            _log.SetRequestId(httpContext.TraceIdentifier);

                            return next.Invoke()
                                       .ContinueWith(task =>
                                                     {
                                                         // Повторная установка контекста логирования ошибок текущего потока,
                                                         // так как ContinueWith работает не в потоке обработки запроса.
                                                         _log.SetRequestId(httpContext.TraceIdentifier);
                                                         _log.SetUserId(httpContext.User?.Identity);

                                                         if (task.IsFaulted)
                                                         {
                                                             LogException(task.Exception);
                                                         }

                                                         LogPerformance($"{httpContext.Request.Method}::{httpContext.Request.Path}", start, task.Exception);

                                                         return Task.CompletedTask;
                                                     });
                        }
                        catch (Exception exception)
                        {
                            LogException(exception);
                            LogPerformance("Invoke", start, exception);

                            return Task.CompletedTask;
                        }
                    });
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
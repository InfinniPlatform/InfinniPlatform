using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Properties;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Hosting layer for processing request errors.
    /// </summary>
    [LoggerName("OWIN")]
    internal class ErrorHandlingAppLayer : IErrorHandlingAppLayer, IDefaultAppLayer
    {
        private readonly ILog _log;
        private readonly IPerformanceLog _perfLog;

        public ErrorHandlingAppLayer(ILog log, IPerformanceLog perfLog)
        {
            _log = log;
            _perfLog = perfLog;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>(_log, _perfLog);
        }


        private class ErrorHandlingMiddleware
        {
            public ErrorHandlingMiddleware(RequestDelegate next, ILog log, IPerformanceLog perfLog)
            {
                _next = next;
                _log = log;
                _perfLog = perfLog;
            }

            private readonly RequestDelegate _next;
            private readonly ILog _log;
            private readonly IPerformanceLog _perfLog;


            public async Task Invoke(HttpContext httpContext)
            {
                var start = DateTime.Now;

                try
                {
                    // Setting logging context for current thread.
                    _log.SetRequestId(httpContext.TraceIdentifier);

                    await _next.Invoke(httpContext)
                               .ContinueWith(task =>
                                             {
                                                 // ContinueWith does not work in same thread as request processing,
                                                 // so setting logging context again.
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
                }
            }

            private void LogException(Exception exception)
            {
                _log.Error(Resources.UnhandledExceptionOwinMiddleware, exception);
            }

            private void LogPerformance(string method, DateTime start, Exception exception)
            {
                _perfLog.Log(method, start, exception);
            }
        }
    }
}
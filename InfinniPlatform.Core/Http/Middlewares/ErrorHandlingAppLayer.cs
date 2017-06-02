using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Properties;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Hosting layer for processing request errors.
    /// </summary>
    public class ErrorHandlingAppLayer : IErrorHandlingAppLayer, IDefaultAppLayer
    {
        public ErrorHandlingAppLayer(ILogger<ErrorHandlingAppLayer> logger, IPerformanceLogger<ErrorHandlingAppLayer> perfLogger)
        {
            _logger = logger;
            _perfLogger = perfLogger;
        }


        private readonly ILogger _logger;
        private readonly IPerformanceLogger _perfLogger;


        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>(this);
        }


        private class ErrorHandlingMiddleware
        {
            public ErrorHandlingMiddleware(RequestDelegate next, ErrorHandlingAppLayer parentLayer)
            {
                _next = next;
                _parentLayer = parentLayer;
            }


            private readonly RequestDelegate _next;
            private readonly ErrorHandlingAppLayer _parentLayer;


            public async Task Invoke(HttpContext httpContext)
            {
                var start = DateTime.Now;

                try
                {
                    await _next.Invoke(httpContext)
                               .ContinueWith(task =>
                                             {
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

                    LogPerformance(nameof(Invoke), start, exception);
                }
            }

            private void LogException(Exception exception)
            {
                _parentLayer._logger.LogError(Resources.UnhandledExceptionOwinMiddleware, exception);
            }

            private void LogPerformance(string method, DateTime start, Exception exception)
            {
                _parentLayer._perfLogger.Log(method, start, exception);
            }
        }
    }
}
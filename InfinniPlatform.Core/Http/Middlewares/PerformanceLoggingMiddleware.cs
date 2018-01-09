using System;
using System.Threading.Tasks;
using InfinniPlatform.Logging;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Logs performance information of request execution.
    /// </summary>
    public class PerformanceLoggingMiddleware
    {
        private readonly IPerformanceLogger<PerformanceLoggingMiddleware> _perfLogger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of <see cref="PerformanceLoggingMiddleware" />.
        /// </summary>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <param name="perfLogger">Performance logger.</param>
        public PerformanceLoggingMiddleware(RequestDelegate next,
                                            IPerformanceLogger<PerformanceLoggingMiddleware> perfLogger)
        {
            _next = next;
            _perfLogger = perfLogger;
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext" /> for the current request.</param>
        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.Now;

            try
            {
                await _next.Invoke(context).ContinueWith(task => LogPerformance(context, start, task.Exception));
            }
            catch (Exception exception)
            {
                await LogPerformance(context, start, exception);
            }
        }

        private Task LogPerformance(HttpContext httpContext, DateTime start, Exception exception)
        {
            // TODO Use ASP.NET log information?
            var method = $"{httpContext.Request.Method}::{httpContext.Request.Path}";

            _perfLogger.Log(method, start, exception);

            return Task.CompletedTask;
        }
    }
}
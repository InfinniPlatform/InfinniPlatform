using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    public class GlobalHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPerformanceLogger<GlobalHandlingMiddleware> _perfLogger;

        public GlobalHandlingMiddleware(RequestDelegate next,
                                        IPerformanceLogger<GlobalHandlingMiddleware> perfLogger)
        {
            _next = next;
            _perfLogger = perfLogger;
        }

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
            var method = $"{httpContext.Request.Method}::{httpContext.Request.Path}";

            _perfLogger.Log(method, start, exception);

            return Task.CompletedTask;
        }
    }
}
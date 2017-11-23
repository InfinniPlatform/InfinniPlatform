using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Hosting layer for processing request errors.
    /// </summary>
    [LoggerName(nameof(GlobalHandlingAppLayer))]
    [Obsolete]
    public class GlobalHandlingAppLayer : IGlobalHandlingAppLayer, IDefaultAppLayer
    {
        public GlobalHandlingAppLayer(IPerformanceLogger<GlobalHandlingAppLayer> perfLogger)
        {
            _perfLogger = perfLogger;
        }


        private readonly IPerformanceLogger _perfLogger;


        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (httpContext, next) =>
            {
                var start = DateTime.Now;

                try
                {
                    await next.Invoke().ContinueWith(task => LogPerformance(httpContext, start, task.Exception));
                }
                catch (Exception exception)
                {
                    await LogPerformance(httpContext, start, exception);
                }
            });
        }

        private Task LogPerformance(HttpContext httpContext, DateTime start, Exception exception)
        {
            var method = $"{httpContext.Request.Method}::{httpContext.Request.Path}";

            _perfLogger.Log(method, start, exception);

            return Task.CompletedTask;
        }
    }
}
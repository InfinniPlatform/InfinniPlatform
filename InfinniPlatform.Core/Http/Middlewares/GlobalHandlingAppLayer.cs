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
    public class GlobalHandlingAppLayer : IGlobalHandlingAppLayer, IDefaultAppLayer
    {
        public GlobalHandlingAppLayer(IPerformanceLogger<GlobalHandlingAppLayer> perfLogger)
        {
            _perfLogger = perfLogger;
        }


        private readonly IPerformanceLogger _perfLogger;


        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalHandlingMiddleware>(this);
        }


        private class GlobalHandlingMiddleware
        {
            public GlobalHandlingMiddleware(RequestDelegate next, GlobalHandlingAppLayer parentLayer)
            {
                _next = next;
                _parentLayer = parentLayer;
            }


            private readonly RequestDelegate _next;
            private readonly GlobalHandlingAppLayer _parentLayer;


            // ReSharper disable once UnusedMember.Local
            public async Task Invoke(HttpContext httpContext)
            {
                var start = DateTime.Now;

                try
                {
                    await _next.Invoke(httpContext).ContinueWith(task => LogPerformance(httpContext, start, task.Exception));
                }
                catch (Exception exception)
                {
                    await LogPerformance(httpContext, start, exception);
                }
            }

            private Task LogPerformance(HttpContext httpContext, DateTime start, Exception exception)
            {
                var method = $"{httpContext.Request.Method}::{httpContext.Request.Path}";

                _parentLayer._perfLogger.Log(method, start, exception);

                return Task.CompletedTask;
            }
        }
    }
}
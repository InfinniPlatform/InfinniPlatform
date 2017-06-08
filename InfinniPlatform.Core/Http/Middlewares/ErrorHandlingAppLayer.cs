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
    [LoggerName(nameof(ErrorHandlingAppLayer))]
    public class ErrorHandlingAppLayer : IErrorHandlingAppLayer, IDefaultAppLayer
    {
        public ErrorHandlingAppLayer(ILogger<ErrorHandlingAppLayer> logger)
        {
            _logger = logger;
        }


        private readonly ILogger _logger;


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


            // ReSharper disable once UnusedMember.Local
            public async Task Invoke(HttpContext httpContext)
            {
                try
                {
                    await _next.Invoke(httpContext).ContinueWith(task => LogException(task.Exception));
                }
                catch (Exception exception)
                {
                    await LogException(exception);
                }
            }

            private Task LogException(Exception exception)
            {
                if (exception != null)
                {
                    _parentLayer._logger.LogError(Resources.UnhandledExceptionOwinMiddleware, exception);
                }

                return Task.CompletedTask;
            }
        }
    }
}
using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Properties;

using Microsoft.AspNetCore.Builder;
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
            app.Use(async (httpContext, next) =>
            {
                try
                {
                    await next.Invoke().ContinueWith(task => LogException(task.Exception));
                }
                catch (Exception exception)
                {
                    await  LogException(exception);
                }
            });
        }

        private Task LogException(Exception exception)
        {
            if (exception != null)
            {
                _logger.LogError(Resources.UnhandledExceptionOwinMiddleware, exception);
            }

            return Task.CompletedTask;
        }
    }
}
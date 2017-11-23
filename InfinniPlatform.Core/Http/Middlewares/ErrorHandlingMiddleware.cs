using System;
using System.Threading.Tasks;

using InfinniPlatform.Properties;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Http.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        public ErrorHandlingMiddleware(RequestDelegate next,
                                       ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context).ContinueWith(task => LogException(task.Exception));
            }
            catch (Exception exception)
            {
                LogException(exception);
            }
        }

        private void LogException(Exception exception)
        {
            if (exception != null)
            {
                _logger.LogError(Resources.UnhandledExceptionOwinMiddleware, exception);
            }
        }
    }
}
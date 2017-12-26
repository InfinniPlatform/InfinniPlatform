using System;
using System.Threading.Tasks;
using InfinniPlatform.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Logs errors occurred in the request execution pipeline.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of <see cref="ErrorHandlingMiddleware" />.
        /// </summary>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <param name="logger">Logger.</param>
        public ErrorHandlingMiddleware(RequestDelegate next,
                                       ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext" /> for the current request.</param>
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
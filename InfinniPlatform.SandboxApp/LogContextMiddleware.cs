using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Serilog.Context;

namespace InfinniPlatform.SandboxApp
{
    /// <summary>
    /// Enrich Serilog log context.
    /// </summary>
    /// <remarks>Should be used after user auth middleware.</remarks>
    public class LogContextMiddleware
    {
        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            LogContext.PushProperty("UserName", context.User?.Identity?.Name);

            await _next.Invoke(context);
        }
    }
}
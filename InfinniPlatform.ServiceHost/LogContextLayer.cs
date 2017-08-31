using System.Threading.Tasks;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace InfinniPlatform.ServiceHost
{
    public class LogContextLayer : IAfterAuthenticationAppLayer, IDefaultAppLayer
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<LogContextMiddleware>(this);
        }

        private class LogContextMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly LogContextLayer _parentLayer;

            public LogContextMiddleware(RequestDelegate next, LogContextLayer parentLayer)
            {
                _next = next;
                _parentLayer = parentLayer;
            }

            // ReSharper disable once UnusedMember.Local
            public async Task Invoke(HttpContext httpContext)
            {
                LogContext.PushProperty("UserName", httpContext.User?.Identity?.Name);

                await _next.Invoke(httpContext);
            }
        }
    }
}
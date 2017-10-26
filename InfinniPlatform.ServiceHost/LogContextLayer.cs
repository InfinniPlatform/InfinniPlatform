using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Serilog.Context;

namespace InfinniPlatform.ServiceHost
{
    public class LogContextLayer : IAfterAuthenticationAppLayer, IDefaultAppLayer
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use((httpContext, next) =>
            {
                LogContext.PushProperty("UserName", httpContext.User?.Identity?.Name);

                return next.Invoke();
            });
        }
    }
}
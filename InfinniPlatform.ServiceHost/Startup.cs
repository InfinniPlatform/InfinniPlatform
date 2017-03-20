using System;
using System.Collections.Generic;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.ServiceHost.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildInfinniServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver)
        {
            var env = resolver.Resolve<IHostingEnvironment>();
            var loggerFactory = resolver.Resolve<ILoggerFactory>();

            var middlewares = resolver.Resolve<IEnumerable<IHttpMiddleware>>();

            foreach (var middleware in middlewares)
            {
                middleware.Configure(app);
            }

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TestMiddleware>();
        }
    }
}
using System;
using System.Collections.Generic;
using InfinniPlatform.Extensions;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            var configureServices = serviceCollection.AddInfAuthentication()
                                                     .AddInfAdfsAuthentication()
                                                     .AddInfCookieAuthentication()
                                                     .AddInfGoogleAuthentication()
                                                     .AddInfFacebookAuthentication()
                                                     .AddInfBlobStorage()
                                                     .AddInfCaching()
                                                     .AddInfDocumentStorage()
                                                     .AddInfMessageQueue()
                                                     .AddInfPrintView()
                                                     .AddInfScheduler()
                                                     .BuildProvider();

            return configureServices;
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

            app.Run(context => context.Response.WriteAsync("InfinniPlatform app started."));
        }
    }
}
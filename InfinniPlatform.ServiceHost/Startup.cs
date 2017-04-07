using System;
using InfinniPlatform.Core.Http.Middlewares;
using InfinniPlatform.Extensions;
using InfinniPlatform.Sdk.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddAuth()
                                            .AddBlobStorage()
                                            .AddCaching()
                                            .AddDocumentStorage()
                                            .AddLog4NetAdapter()
                                            .AddMessageQueue()
                                            .AddPrintView()
                                            .AddScheduler()
                                            .BuildProvider();

            return configureServices;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.AddApplicationMiddleware(new NancyMiddlewareOptions {PerformPassThrough = true});
            app.UseInfinniMiddlewares(resolver);
        }
    }
}
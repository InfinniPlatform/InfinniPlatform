using System;
using System.Collections.Generic;
using InfinniPlatform.Extensions;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            var configureServices = serviceCollection.AddAuth()
                                                     .AddAuthAdfs()
                                                     .AddAuthCookie()
                                                     .AddAuthGoogle()
                                                     .AddAuthFacebook()
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

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment appEnv, IApplicationLifetime lifetime)
        {
            var middlewares = resolver.Resolve<IEnumerable<IHttpMiddleware>>();

            foreach (var middleware in middlewares)
            {
                middleware.Configure(app);
            }
        }
    }
}
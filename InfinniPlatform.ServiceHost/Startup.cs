using System;
using System.Collections.Generic;
using InfinniPlatform.Core.IoC;
using InfinniPlatform.Extensions;
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
        public IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            // #1
            //var serviceProvider = new InfinniPlatformServiceProvider(serviceCollection);

            //serviceProvider.AddDocumentStorage()
            //               .AddAuthentication();

            //return serviceProvider;

            // #2
            //var serviceProvider = new InfinniPlatformServiceProviderFactory(serviceCollection);

            //serviceProvider.AddDocumentStorage()
            //               .AddAuthentication();

            //return serviceProvider.Create();

            // #3
            //return new InfinniPlatformServiceProviderFactory(serviceCollection)
            //        .AddDocumentStorage()
            //        .AddAuthentication()
            //        .Create();

            // #4
            //return serviceCollection.AddDocumentStorage()
            //                        .AddAuthentication()
            //                        .Create();

            var configureServices = serviceCollection.AddInfDocumentStorage()
                                                     .AddInfAuthentication()
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

            app.UseMiddleware<TestMiddleware>();
        }
    }
}
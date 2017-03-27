using System;
using System.Collections.Generic;
using InfinniPlatform.Extensions;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Hosting;
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

        public void Configure(IApplicationBuilder appBuilder, IContainerResolver resolver, IHostingEnvironment appEnv, IApplicationLifetime appLifetime)
        {
            var appStartedHandlers = resolver.Resolve<IEnumerable<IAppStartedHandler>>();
            var appStoppedHandlers = resolver.Resolve<IEnumerable<IAppStoppedHandler>>();

            foreach (var handler in appStartedHandlers)
            {
                appLifetime.ApplicationStarted.Register(handler.Handle);
            }

            foreach (var handler in appStoppedHandlers)
            {
                appLifetime.ApplicationStopped.Register(handler.Handle);
            }

            var middlewares = resolver.Resolve<IEnumerable<IHttpMiddleware>>();

            foreach (var middleware in middlewares)
            {
                middleware.Configure(appBuilder);
            }
        }
    }
}
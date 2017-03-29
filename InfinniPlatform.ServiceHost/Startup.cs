using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Auth.Internal.Identity.MongoDb;
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddAuth()
                                            //.AddAuthAdfs()
                                            //.AddAuthCookie()
                                            //.AddAuthGoogle()
                                            //.AddAuthFacebook()
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

        public void Configure(IApplicationBuilder builder, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            var appStartedHandlers = resolver.Resolve<IEnumerable<IAppStartedHandler>>();
            var appStoppedHandlers = resolver.Resolve<IEnumerable<IAppStoppedHandler>>();

            foreach (var handler in appStartedHandlers)
            {
                lifetime.ApplicationStarted.Register(handler.Handle);
            }

            foreach (var handler in appStoppedHandlers)
            {
                lifetime.ApplicationStopped.Register(handler.Handle);
            }

            var middlewares = resolver.Resolve<IEnumerable<IHttpMiddleware>>();
            var httpMiddlewares = middlewares.OrderBy(middleware => middleware.Type).ToArray();

            foreach (var middleware in httpMiddlewares)
            {
                middleware.Configure(builder);
            }
        }
    }
}
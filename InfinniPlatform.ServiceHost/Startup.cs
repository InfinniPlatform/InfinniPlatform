using System;
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
            app.AddAuthenticationBarrierMiddleware(() =>
                                                   {
                                                       var facebookOptions = new FacebookOptions
                                                                             {
                                                                                 AppId = "199994547162009",
                                                                                 AppSecret = "ffd317eb16b31540f42c3bbc406bedfa"
                                                                             };

                                                       var microsoftAccountOptions = new MicrosoftAccountOptions
                                                                                     {
                                                                                         ClientId = "51ce0ff9-13d3-4d51-b6ee-d8f6a4c7061c",
                                                                                         ClientSecret = "bco1bSU7bX7cfprfBQrkCA8"
                                                                                     };

                                                       app.UseFacebookAuthentication(facebookOptions)
                                                          .UseMicrosoftAccountAuthentication(microsoftAccountOptions);
                                                   });

            app.UseInfinniMiddlewares(resolver, lifetime);
        }
    }
}
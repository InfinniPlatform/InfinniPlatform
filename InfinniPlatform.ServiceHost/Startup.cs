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
                                            .AddAuthCookie()
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
            app.UseInfinniMiddlewares(resolver, lifetime);

//            app.UseExternalAuth(() =>
//                                {
//                                    var facebookOptions = new FacebookOptions
//                                                          {
//                                                              AppId = Configuration["Authentication:Facebook:AppId"],
//                                                              AppSecret = Configuration["Authentication:Facebook:AppSecret"]
//                                                          };
//
//                                    var microsoftAccountOptions = new MicrosoftAccountOptions
//                                                                  {
//                                                                      ClientId = Configuration["Authentication:Microsoft:AppId"],
//                                                                      ClientSecret = Configuration["Authentication:Microsoft:AppSecret"]
//                                                                  };
//
//                                    app.UseFacebookAuthentication(facebookOptions)
//                                       .UseMicrosoftAccountAuthentication(microsoftAccountOptions);
//                                });
//
        }
    }
}
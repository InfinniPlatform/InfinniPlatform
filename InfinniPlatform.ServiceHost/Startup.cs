using System;
using System.Collections.Generic;
using System.IO;
using InfinniPlatform.AspNetCore;
using InfinniPlatform.Extensions;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using InfinniPlatform.ServiceHost.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("AppConfig.json", true, true)
                    .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddLog4NetLogging()
                                            .AddAuthInternal(_configuration)
                                            .AddAuthHttpService()
                                            .AddInMemoryCache()
                                            .AddRedisSharedCache(_configuration)
                                            .AddTwoLayerCache(_configuration)
                                            .AddFileSystemBlobStorage(_configuration)
                                            .AddBlobStorageHttpService()
                                            .AddMongoDocumentStorage(_configuration)
                                            .AddDocumentStorageHttpService()
                                            .AddRabbitMqMessageQueue(_configuration)
                                            .AddQuartzScheduler(_configuration)
                                            .AddSchedulerHttpService()
                                            .AddPrintView(_configuration)
                                            .AddHeartbeatHttpService()
                                            .BuildProvider(_configuration);

            return configureServices;
        }

        public void Configure(IApplicationBuilder app, IContainerResolver resolver)
        {
            app.UseStaticFilesMapping(_configuration);

            app.UseInfinniMiddlewares(resolver);
        }
    }
}
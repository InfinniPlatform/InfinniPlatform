using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("AppConfig.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }


        private readonly IConfigurationRoot _configuration;


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddAuth()
                                            .AddLog4NetLogging()
                                            .AddInMemoryCache()
                                            .AddRedisSharedCache()
                                            .AddTwoLayerCache()
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

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}
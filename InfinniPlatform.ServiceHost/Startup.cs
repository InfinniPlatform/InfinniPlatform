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

        public void Configure(IApplicationBuilder app, IContainerResolver resolver, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseInfinniMiddlewares(resolver);
        }
    }
}
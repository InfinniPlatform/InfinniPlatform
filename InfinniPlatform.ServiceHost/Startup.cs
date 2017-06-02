using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            const string outputTemplate = "{Timestamp:o}|{Level:u3}|{SourceContext}|{Message}{NewLine}{Exception}";

            // Example of configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole(outputTemplate: outputTemplate)
                .CreateLogger();

            // Example of configure application
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("AppConfig.json", true, true)
                    .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configureServices = services.AddAuthInternal(_configuration)
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

        public void Configure(IApplicationBuilder app,
                              IContainerResolver resolver,
                              ILoggerFactory loggerFactory,
                              IApplicationLifetime appLifetime)
        {
            // Register Serilog
            loggerFactory.AddSerilog();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            // Setup default application layers
            app.UseDefaultAppLayers(resolver);
        }
    }
}
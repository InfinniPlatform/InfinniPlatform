using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace InfinniPlatform.ServiceHost
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            // Example of configure application
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("AppConfig.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"AppConfig.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.AddAuthInternal(_configuration)
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
                                          .AddContainerModule(new ContainerModule())
                                          .BuildProvider(_configuration);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app,
                              IContainerResolver resolver,
                              ILoggerFactory loggerFactory,
                              IApplicationLifetime appLifetime,
                              IHttpContextAccessor httpContextAccessor)
        {
            ConfigureLogger(loggerFactory, appLifetime, httpContextAccessor);

            app.UseStaticFilesMapping(_configuration);

            // Setup default application layers
            app.UseDefaultAppLayers(resolver);
        }


        private static void ConfigureLogger(ILoggerFactory loggerFactory,
                                            IApplicationLifetime appLifetime,
                                            IHttpContextAccessor httpContextAccessor)
        {
            const string outputTemplate = "{Timestamp:o}|{Level:u3}|{RequestId}|{UserName}|{SourceContext}|{Message}{NewLine}{Exception}";
            const string outputTemplatePerf = "{Timestamp:o}|{RequestId}|{UserName}|{SourceContext}|null|null|{Message}{NewLine}";

            Func<LogEvent, bool> performanceLoggerFilter = 
                Matching.WithProperty<string>(
                    Constants.SourceContextPropertyName,
                    p => p.StartsWith(nameof(IPerformanceLogger)));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(new HttpContextLogEventEnricher(httpContextAccessor))
                .WriteTo.LiterateConsole(outputTemplate: outputTemplate)
                .WriteTo.Logger(lc => lc.Filter.ByExcluding(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/events-{Date}.log",
                                                             outputTemplate: outputTemplate))
                .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(performanceLoggerFilter)
                                        .WriteTo.RollingFile("logs/performance-{Date}.log",
                                                             outputTemplate: outputTemplatePerf))
                .CreateLogger();

            // Register Serilog
            loggerFactory.AddSerilog();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}
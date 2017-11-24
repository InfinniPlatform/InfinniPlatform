using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.SandboxApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvcWithInternalServices();

            var serviceProvider = services.AddAuthInternal(_configuration)
                                          .AddAuthHttpService(mvcBuilder)
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
                              IContainerResolver resolver)
        {
            app.UseStaticFilesMapping(_configuration, resolver)
               .UsePerformanceLogging()
               .UseErrorHandling()
               .UseAuthInternal()
               .UseMiddleware<LogContextMiddleware>()
               .UseMvcWithInternalServices();

            CoreExtensions.RegisterAppLifetimeHandlers(resolver);
        }
    }
}
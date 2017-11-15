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
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices();

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
                              IContainerResolver resolver)
        {
            app.UseStaticFilesMapping(_configuration, resolver);

            var httpServices = resolver.Resolve<EntityDocumentHttpController>();

            // Setup default application layers
            app.UseDefaultAppLayers(resolver);
            app.UseMvc();
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using InfinniPlatform.AspNetCore;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.ServiceHost
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
            Type[] entityTypes = {typeof(Entity)};
            var closedControllerTypes = entityTypes
                .Select(et => typeof(DocumentHttpController<>).MakeGenericType(et))
                .Select(cct => cct.GetTypeInfo())
                .ToArray();

            services.AddMvc()
                    .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new GenericControllerApplicationPart(closedControllerTypes)));

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

            // Setup default application layers
            app.UseDefaultAppLayers(resolver);
            app.UseMvc();
        }
    }
}
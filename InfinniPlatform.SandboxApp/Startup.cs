using System;
using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.AspNetCore;
using InfinniPlatform.Diagnostics;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
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

            var assembliesDirectory = Path.GetDirectoryName(typeof(Startup).Assembly.Location);

            var enumerateFiles = Directory.GetFiles(assembliesDirectory, "*.dll");
            foreach (var dll in enumerateFiles)
            {
                var assembly = Assembly.LoadFile(dll);
                var controllerTypes = assembly.GetTypes()
                                              .Where(type => typeof(Controller).IsAssignableFrom(type))
                                              .ToArray();
            }

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
            app.UseDefaultAppLayers(resolver);
            app.UseMvc();

            var controller = resolver.Resolve<SystemInfoHttpService>();
        }
    }
}
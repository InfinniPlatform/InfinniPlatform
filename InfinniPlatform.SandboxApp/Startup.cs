using System;

using InfinniPlatform.AspNetCore;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            var mvcBuilder = services.AddMvc()
                                     .AddJsonOptions(json =>
                                                     {
                                                         var settings = json.SerializerSettings;

                                                         settings.Formatting = Formatting.Indented;
                                                         settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                                                         settings.NullValueHandling = NullValueHandling.Ignore;
                                                         settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() };
                                                         settings.Converters.Add(new DateJsonConverter());
                                                         settings.Converters.Add(new TimeJsonConverter());
                                                         settings.Converters.Add(new DynamicDocumentJsonConverter());
                                                     });

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
            app.UseStaticFilesMapping(_configuration, resolver);
            app.UseDefaultAppLayers(resolver);
            app.UseMvc();
        }
    }
}
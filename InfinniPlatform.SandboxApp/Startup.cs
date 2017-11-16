using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.AspNetCore;
using InfinniPlatform.Diagnostics;
using InfinniPlatform.DocumentStorage;
using InfinniPlatform.Http.StaticFiles;
using InfinniPlatform.IoC;
using InfinniPlatform.SandboxApp.Models;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.SandboxApp
{
    public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            TypeInfo[] types = {typeof(Entity).GetTypeInfo() };

            foreach (var type in types)
            {
                var typeName = $"{type.Name}Controller";

                if (feature.Controllers.All(t => t.Name != typeName))
                {
                    // There's no 'real' controller for this entity, so add the generic version.
                    var controllerType = typeof(DocumentController<>).MakeGenericType(type.AsType()).GetTypeInfo();

                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }

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
                .AddJsonOptions(json =>
                    {
                        var settings = json.SerializerSettings;

                        settings.Formatting = Formatting.Indented;
                        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        settings.ContractResolver = new DefaultContractResolver(){NamingStrategy = new DefaultNamingStrategy()};
                        settings.Converters.Add(new DateJsonConverter());
                        settings.Converters.Add(new TimeJsonConverter());
                        settings.Converters.Add(new DynamicDocumentJsonConverter());
                    })
                    .ConfigureApplicationPartManager(man=>man.FeatureProviders.Add(new GenericControllerFeatureProvider()));

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
        }
    }
}
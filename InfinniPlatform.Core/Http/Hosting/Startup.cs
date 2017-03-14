using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Core.Http.Hosting
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, HttpMiddleware[] httpMiddlewares)
        {
            foreach (var httpMiddleware in httpMiddlewares)
            {
                httpMiddleware.Configure(app);
            }

            //TODO Register OWIN layers.
            //TODO Register Nancy bootstrapper.
            //app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new HttpServiceNancyBootstrapper()));
        }
    }
}
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.Core.Http.Hosting
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            var containerResolverFactory = new AutofacHttpContainerResolverFactory();
            var containerResolver = containerResolverFactory.CreateContainerResolver();
            
            //builder.RegisterModule(new AutofacContainerModule());

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
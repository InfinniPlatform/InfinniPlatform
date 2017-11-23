using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using InfinniPlatform.Hosting;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;
using InfinniPlatform.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class CoreExtensions
    {
        /// <summary>
        /// Adds an application IoC-container module.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="containerModule">The application IoC-container module.</param>
        public static IServiceCollection AddContainerModule(this IServiceCollection services, IContainerModule containerModule)
        {
            return services.AddSingleton(provider => containerModule);
        }


        /// <summary>
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services)
        {
            var options = AppOptions.Default;

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">The application configuration.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(AppOptions.SectionName).Get<AppOptions>();

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="options">The general application settings.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, AppOptions options)
        {
            // Because IoC uses IHttpContextAccessor for InstancePerLifetimeScope strategy
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // For correct resolving a logger name via LoggerNameAttribute
            services.AddSingleton(typeof(ILogger<>), typeof(TypedLogger<>));

            services.AddSingleton(provider => new CoreContainerModule(options ?? AppOptions.Default));

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            RegisterDefinedModules(containerBuilder, services);

            // ReSharper disable AccessToModifiedClosure

            // Register Autofac-container itself
            IContainer autofacRootContainer = null;
            containerBuilder.RegisterInstance((Func<IContainer>)(() => autofacRootContainer));
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Register wrapper for container Регистрация обертки над контейнером
            containerBuilder.RegisterType<ContainerServiceRegistry>().As<IContainerServiceRegistry>().SingleInstance();
            containerBuilder.RegisterType<ServiceProviderAccessor>().As<IServiceProviderAccessor>().SingleInstance();
            containerBuilder.RegisterType<ContainerResolver>().As<IContainerResolver>().SingleInstance();

            // Построение контейнера зависимостей
            autofacRootContainer = containerBuilder.Build();

            return new AutofacServiceProvider(autofacRootContainer);
        }

        private static void RegisterDefinedModules(ContainerBuilder containerBuilder, IServiceCollection services)
        {
            var modules = services.Where(service => typeof(IContainerModule).IsAssignableFrom(service.ServiceType));

            foreach (var module in modules)
            {
                var containerModule = (IContainerModule)module.ImplementationFactory.Invoke(null);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        public static IApplicationBuilder UseMvcWithInternalServices(this IApplicationBuilder app)
        {
            return app.UseMvc();
        }

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }

        public static IApplicationBuilder UseGlobalHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalHandlingMiddleware>();
        }


        public static void RegisterAppLifetimeHandlers(IContainerResolver resolver)
        {
            var appStartedHandlers = resolver.Resolve<IAppStartedHandler[]>();
            var appStoppedHandlers = resolver.Resolve<IAppStoppedHandler[]>();
            var lifetime = resolver.Resolve<IApplicationLifetime>();

            foreach (var handler in appStartedHandlers)
            {
                lifetime.ApplicationStarted.Register(handler.Handle);
            }

            foreach (var handler in appStoppedHandlers)
            {
                lifetime.ApplicationStopped.Register(handler.Handle);
            }
        }
    }
}
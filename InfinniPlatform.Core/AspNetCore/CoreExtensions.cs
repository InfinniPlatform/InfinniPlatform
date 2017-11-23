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

            // Регистрация самого Autofac-контейнера
            IContainer autofacRootContainer = null;
            containerBuilder.RegisterInstance((Func<IContainer>)(() => autofacRootContainer));
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Регистрация обертки над контейнером
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


        /// <summary>
        /// Registers application layers defined by user in IoC.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="resolver">The IoC container resolver.</param>
        public static void UseAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            app.ConfigureAppLayers(resolver.Resolve<IGlobalHandlingAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IErrorHandlingAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IBeforeAuthenticationAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IAuthenticationBarrierAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IExternalAuthenticationAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IInternalAuthenticationAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IAfterAuthenticationAppLayer[]>());
            app.ConfigureAppLayers(resolver.Resolve<IBusinessAppLayer[]>());

            RegisterAppLifetimeHandlers(resolver);
        }

        /// <summary>
        /// Registers default application layers.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="resolver">The IoC container resolver.</param>
        public static void UseDefaultAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            var layers = resolver.Resolve<IDefaultAppLayer[]>();

            // ReSharper disable SuspiciousTypeConversion.Global

            app.ConfigureAppLayers(layers.OfType<IGlobalHandlingAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IErrorHandlingAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IBeforeAuthenticationAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IAuthenticationBarrierAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IExternalAuthenticationAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IInternalAuthenticationAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IAfterAuthenticationAppLayer>());
            app.ConfigureAppLayers(layers.OfType<IBusinessAppLayer>());

            // ReSharper restore SuspiciousTypeConversion.Global

            RegisterAppLifetimeHandlers(resolver);
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

        private static void ConfigureAppLayers<T>(this IApplicationBuilder app, IEnumerable<T> appLayers) where T : IAppLayer
        {
            foreach (var layer in appLayers)
            {
                layer.Configure(app);
            }
        }
    }
}
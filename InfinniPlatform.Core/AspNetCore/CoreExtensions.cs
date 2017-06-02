using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = AppOptions.Default;

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IConfigurationRoot configuration, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = configuration.GetSection(AppOptions.SectionName).Get<AppOptions>();

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider"/> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Application options.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, AppOptions options, IEnumerable<IContainerModule> containerModules = null)
        {
            // Because IoC uses IHttpContextAccessor for InstancePerLifetimeScope strategy
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // For correct resolving a logger name via LoggerNameAttribute
            services.AddSingleton(typeof(ILogger<>), typeof(TypedLogger<>));

            services.AddSingleton(provider => new CoreContainerModule(options ?? AppOptions.Default));

            if (containerModules != null)
            {
                foreach (var containerModule in containerModules)
                {
                    services.AddSingleton(provider => containerModule);
                }
            }

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            RegisterDefinedModules(containerBuilder, services);
            RegisterUserModules(containerBuilder);

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


        /// <summary>
        /// Register application layers defined by user in IoC.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void UseAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            app.RegisterAppLayers<IGlobalHandlingAppLayer>(resolver);
            app.RegisterAppLayers<IErrorHandlingAppLayer>(resolver);
            app.RegisterAppLayers<IBeforeAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IAuthenticationBarrierAppLayer>(resolver);
            app.RegisterAppLayers<IExternalAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IInternalAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IAfterAuthenticationAppLayer>(resolver);
            app.RegisterAppLayers<IBusinessAppLayer>(resolver);
        }

        /// <summary>
        /// Register default application layers.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void UseDefaultAppLayers(this IApplicationBuilder app, IContainerResolver resolver)
        {
            var layers = resolver.Resolve<IDefaultAppLayer[]>();

            // ReSharper disable SuspiciousTypeConversion.Global

            foreach (var layer in layers.OfType<IGlobalHandlingAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IErrorHandlingAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IBeforeAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IAuthenticationBarrierAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IExternalAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IInternalAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IAfterAuthenticationAppLayer>())
            {
                layer.Configure(app);
            }

            foreach (var layer in layers.OfType<IBusinessAppLayer>())
            {
                layer.Configure(app);
            }

            // ReSharper restore SuspiciousTypeConversion.Global
        }

        /// <summary>
        /// Register application lifetime handlers.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="resolver">IoC container resolver.</param>
        public static void RegisterAppLifetimeHandlers(this IApplicationBuilder app, IContainerResolver resolver)
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

        private static void RegisterUserModules(ContainerBuilder containerBuilder)
        {
            var modules = Assembly.GetEntryAssembly()
                                  .GetTypes()
                                  .Where(type => typeof(IContainerModule).IsAssignableFrom(type));

            foreach (var module in modules)
            {
                var containerModule = (IContainerModule)Activator.CreateInstance(module);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        private static void RegisterAppLayers<T>(this IApplicationBuilder app, IContainerResolver resolver) where T : IAppLayer
        {
            var appLayers = resolver.Resolve<T[]>();

            foreach (var layer in appLayers)
            {
                layer.Configure(app);
            }
        }
    }
}
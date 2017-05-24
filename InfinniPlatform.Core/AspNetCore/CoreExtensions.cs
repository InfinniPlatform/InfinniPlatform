using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using InfinniPlatform.Hosting;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class CoreExtensions
    {
        public static IServiceProvider BuildProvider(this IServiceCollection services, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = AppOptions.Default;

            return BuildProvider(services, options);
        }

        public static IServiceProvider BuildProvider(this IServiceCollection services, IConfigurationRoot configuration, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = configuration.GetSection(AppOptions.SectionName)
                                       .Get<AppOptions>();

            return BuildProvider(services, options);
        }

        public static IServiceProvider BuildProvider(this IServiceCollection services, AppOptions options, IEnumerable<IContainerModule> containerModules = null)
        {
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

        /// <summary>
        /// Регистрирует middlewares обработки запросов к приложению.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="resolver">Провайдер разрешения зависимостей.</param>
        public static void UseInfinniMiddlewares(this IApplicationBuilder app, IContainerResolver resolver)
        {
            RegisterAppLifetimeHandlers(resolver);
            RegisterMiddlewares(app, resolver);
        }

        private static void RegisterAppLifetimeHandlers(IContainerResolver resolver)
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

        private static void RegisterMiddlewares(IApplicationBuilder app, IContainerResolver resolver)
        {
            var middlewares = resolver.Resolve<IMiddleware[]>();

            foreach (var middleware in middlewares.OfType<IGlobalHandlingMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IErrorHandlingMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IBeforeAuthenticationMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IAuthenticationBarrierMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IExternalAuthenticationMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IInternalAuthenticationMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IAfterAuthenticationMiddleware>())
            {
                middleware.Configure(app);
            }

            foreach (var middleware in middlewares.OfType<IApplicationMiddleware>())
            {
                middleware.Configure(app);
            }
        }
    }
}
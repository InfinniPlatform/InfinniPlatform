using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.IoC;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Создает провайдер на основе коллекции зарегистрированных сервисов.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection serviceCollection)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            containerBuilder.RegisterCoreModules();
            containerBuilder.RegisterPlatformModules(serviceCollection);
            containerBuilder.RegisterUserDefinedModules();

            // ReSharper disable AccessToModifiedClosure

            // Регистрация самого Autofac-контейнера
            IContainer autofacRootContainer = null;
            Func<IContainer> autofacRootContainerFactory = () => autofacRootContainer;
            containerBuilder.RegisterInstance(autofacRootContainerFactory);
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // Регистрация обертки над контейнером
            IContainerResolver containerResolver = null;
            Func<IContainerResolver> containerResolverFactory = () => containerResolver;
            containerBuilder.RegisterInstance(containerResolverFactory);
            containerBuilder.Register(r => r.Resolve<Func<IContainerResolver>>()()).As<IContainerResolver>().SingleInstance();

            // Регистрация контейнера для Asp.Net
            IServiceProvider serviceProvider = null;
            Func<IServiceProvider> serviceProviderFactory = () => serviceProvider;
            containerBuilder.RegisterInstance(serviceProviderFactory);
            containerBuilder.Register(r => r.Resolve<Func<IServiceProvider>>()()).As<IServiceProvider>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = containerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);
            serviceProvider = new AutofacServiceProvider(autofacRootContainer);

            return serviceProvider;
        }

        /// <summary>
        /// Регистрирует правила CORS проверки.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddInfinniCors(this IServiceCollection serviceCollection)
        {
            // TODO: Сделать правила CORS проверки конфигурируемыми.
            serviceCollection.AddCors(options => { options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()); });

            return serviceCollection;
        }

        public static void UseInfinniMiddlewares(this IApplicationBuilder builder, IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            var appStartedHandlers = resolver.Resolve<IEnumerable<IAppStartedHandler>>();
            var appStoppedHandlers = resolver.Resolve<IEnumerable<IAppStoppedHandler>>();

            foreach (var handler in appStartedHandlers)
            {
                lifetime.ApplicationStarted.Register(handler.Handle);
            }

            foreach (var handler in appStoppedHandlers)
            {
                lifetime.ApplicationStopped.Register(handler.Handle);
            }

            var middlewares = resolver.Resolve<IEnumerable<IHttpMiddleware>>();
            var httpMiddlewares = middlewares.OrderBy(middleware => middleware.Type).ToArray();

            foreach (var middleware in httpMiddlewares)
            {
                middleware.Configure(builder);
            }
        }


        private static void RegisterCoreModules(this ContainerBuilder containerBuilder)
        {
            var coreModules = typeof(CoreContainerModule).GetTypeInfo().Assembly
                                                         .GetTypes()
                                                         .Where(type => typeof(IContainerModule).IsAssignableFrom(type));

            foreach (var module in coreModules)
            {
                var containerModule = (IContainerModule) Activator.CreateInstance(module);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        private static void RegisterPlatformModules(this ContainerBuilder containerBuilder, IServiceCollection serviceCollection)
        {
            var userModules = serviceCollection.Where(service => typeof(IContainerModule).IsAssignableFrom(service.ServiceType));

            foreach (var module in userModules)
            {
                var containerModule = (IContainerModule) module.ImplementationFactory.Invoke(null);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        private static void RegisterUserDefinedModules(this ContainerBuilder containerBuilder)
        {
            var userModules = Assembly.GetEntryAssembly()
                                      .GetTypes()
                                      .Where(type => typeof(IContainerModule).IsAssignableFrom(type));

            foreach (var module in userModules)
            {
                var containerModule = (IContainerModule)Activator.CreateInstance(module);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }
    }
}
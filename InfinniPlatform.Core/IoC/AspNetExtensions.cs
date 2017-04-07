using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.Core.Http.Middlewares;
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
        private static readonly Dictionary<HttpMiddlewareType, IMiddlewareOptions> MiddlewareOptions = new Dictionary<HttpMiddlewareType, IMiddlewareOptions>();

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
            containerBuilder.RegisterInstance((Func<IContainer>) (() => autofacRootContainer));
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // Регистрация обертки над контейнером
            IContainerResolver containerResolver = null;
            containerBuilder.RegisterInstance((Func<IContainerResolver>) (() => containerResolver));
            containerBuilder.Register(r => r.Resolve<Func<IContainerResolver>>()()).As<IContainerResolver>().SingleInstance();

            // Регистрация контейнера для Asp.Net
            IServiceProvider serviceProvider = null;
            containerBuilder.RegisterInstance((Func<IServiceProvider>) (() => serviceProvider));
            containerBuilder.Register(r => r.Resolve<Func<IServiceProvider>>()()).As<IServiceProvider>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            // Построение контейнера зависимостей
            autofacRootContainer = containerBuilder.Build();
            containerResolver = new AutofacContainerResolver(autofacRootContainer, AutofacRequestLifetimeScopeOwinMiddleware.TryGetRequestContainer);
            serviceProvider = new AutofacServiceProvider(autofacRootContainer);

            return serviceProvider;
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

        /// <summary>
        /// Регистрирует middleware глобальной обработки запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddGlobalHandlingMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.GlobalHandling, options);
        }

        /// <summary>
        /// Регистрирует middleware обработки ошибок выполнения запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddErrorHandlingMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.ErrorHandling, options);
        }

        /// <summary>
        /// Регистрирует middleware обработки запросов до аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddBeforeAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.BeforeAuthentication, options);
        }

        /// <summary>
        /// Регистрирует middleware барьерной аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddAuthenticationBarrierMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.AuthenticationBarrier, options);
        }

        /// <summary>
        /// Регистрирует middleware аутентификации пользователя на основе внешнего провайдера.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddExternalAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.ExternalAuthentication, options);
        }

        /// <summary>
        /// Регистрирует middleware аутентификации пользователя средствами приложения.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddInternalAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.InternalAuthentication, options);
        }

        /// <summary>
        /// Регистрирует middleware обработки запросов после аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddAfterAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.AfterAuthentication, options);
        }

        /// <summary>
        /// Регистрирует middleware обработки прикладных запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddApplicationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.Application, options);
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
            var httpMiddlewares = resolver.Resolve<IHttpMiddleware[]>();
            var httpMiddlewareTypes = (HttpMiddlewareType[])Enum.GetValues(typeof(HttpMiddlewareType));

            foreach (var type in httpMiddlewareTypes)
            {
                var options = MiddlewareOptions[type];

                // Регистрация системных middlewares.
                foreach (var middleware in httpMiddlewares.Where(m => m.Type == type))
                {
                    middleware.Configure(app, options);
                }

                // Регистрация пользовательских middlewares.
                if (MiddlewareOptions.ContainsKey(type))
                {
                    options.Configure(app);
                }
            }
        }

        private static void RegisterCoreModules(this ContainerBuilder containerBuilder)
        {
            var coreModules = typeof(CoreContainerModule).GetTypeInfo()
                                                         .Assembly
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
                var containerModule = (IContainerModule) Activator.CreateInstance(module);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        private static void AddUserMiddleware(HttpMiddlewareType key, IMiddlewareOptions value)
        {
            MiddlewareOptions[key] = value;
        }
    }
}
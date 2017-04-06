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
        private static readonly Dictionary<HttpMiddlewareType, Action<IApplicationBuilder>> UserMiddlewareActions = new Dictionary<HttpMiddlewareType, Action<IApplicationBuilder>>();

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
        /// <param name="lifetime">Конфигуратор поведения приложения при запуске/остановке.</param>
        public static void UseInfinniMiddlewares(this IApplicationBuilder app, IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            RegisterAppLifetimeHandlers(resolver, lifetime);
            RegisterMiddlewares(app, resolver);
        }

        /// <summary>
        /// Регистрирует middleware глобальной обработки запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddGlobalHandlingMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.GlobalHandling, action);
        }

        /// <summary>
        /// Регистрирует middleware обработки ошибок выполнения запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddErrorHandlingMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.ErrorHandling, action);
        }

        /// <summary>
        /// Регистрирует middleware обработки запросов до аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddBeforeAuthenticationMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.BeforeAuthentication, action);
        }

        /// <summary>
        /// Регистрирует middleware барьерной аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddAuthenticationBarrierMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.AuthenticationBarrier, action);
        }

        /// <summary>
        /// Регистрирует middleware аутентификации пользователя на основе внешнего провайдера.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddExternalAuthenticationMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.ExternalAuthentication, action);
        }

        /// <summary>
        /// Регистрирует middleware аутентификации пользователя средствами приложения.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddInternalAuthenticationMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.InternalAuthentication, action);
        }

        /// <summary>
        /// Регистрирует middleware обработки запросов после аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddAfterAuthenticationMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.AfterAuthentication, action);
        }

        /// <summary>
        /// Регистрирует middleware обработки прикладных запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="action">Обработчик запроса.</param>
        public static void AddApplicationMiddleware(this IApplicationBuilder app, Action<IApplicationBuilder> action)
        {
            AddUserMiddleware(HttpMiddlewareType.Application, action);
        }

        private static void RegisterAppLifetimeHandlers(IContainerResolver resolver, IApplicationLifetime lifetime)
        {
            var appStartedHandlers = resolver.Resolve<IAppStartedHandler[]>();
            var appStoppedHandlers = resolver.Resolve<IAppStoppedHandler[]>();

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
                // Регистрация системных middlewares.
                foreach (var middleware in httpMiddlewares.Where(m => m.Type == type))
                {
                    middleware.Configure(app);
                }

                // Регистрация пользовательских middlewares.
                if (UserMiddlewareActions.ContainsKey(type))
                {
                    UserMiddlewareActions[type](app);
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

        private static void AddUserMiddleware(HttpMiddlewareType key, Action<IApplicationBuilder> value)
        {
            if (UserMiddlewareActions.ContainsKey(key))
            {
                UserMiddlewareActions[key] += value;
            }
            else
            {
                UserMiddlewareActions.Add(key, value);
            }
        }
    }
}
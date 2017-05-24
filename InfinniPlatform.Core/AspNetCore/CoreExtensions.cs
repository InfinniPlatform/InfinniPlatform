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
        private static readonly Dictionary<HttpMiddlewareType, IMiddlewareOptions> MiddlewareOptions = BuildDefaultMiddlewareOptions();

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
            containerBuilder.RegisterInstance((Func<IContainer>) (() => autofacRootContainer));
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
                var containerModule = (IContainerModule) module.ImplementationFactory.Invoke(null);
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
                var containerModule = (IContainerModule) Activator.CreateInstance(module);
                var autofacContainerModule = new AutofacContainerModule(containerModule);
                containerBuilder.RegisterModule(autofacContainerModule);
            }
        }

        private static Dictionary<HttpMiddlewareType, IMiddlewareOptions> BuildDefaultMiddlewareOptions()
        {
            var httpMiddlewareTypes = (HttpMiddlewareType[]) Enum.GetValues(typeof(HttpMiddlewareType));

            return httpMiddlewareTypes.ToDictionary<HttpMiddlewareType, HttpMiddlewareType, IMiddlewareOptions>(type => type, type => null);
        }

        /// <summary>
        ///     Регистрирует middlewares обработки запросов к приложению.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="resolver">Провайдер разрешения зависимостей.</param>
        public static void UseInfinniMiddlewares(this IApplicationBuilder app, IContainerResolver resolver)
        {
            RegisterAppLifetimeHandlers(resolver);
            RegisterMiddlewares(app, resolver);
        }

        /// <summary>
        ///     Регистрирует middleware глобальной обработки запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddGlobalHandlingMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.GlobalHandling, options);
        }

        /// <summary>
        ///     Регистрирует middleware обработки ошибок выполнения запросов.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddErrorHandlingMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.ErrorHandling, options);
        }

        /// <summary>
        ///     Регистрирует middleware обработки запросов до аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddBeforeAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.BeforeAuthentication, options);
        }

        /// <summary>
        ///     Регистрирует middleware барьерной аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddAuthenticationBarrierMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.AuthenticationBarrier, options);
        }

        /// <summary>
        ///     Регистрирует middleware аутентификации пользователя на основе внешнего провайдера.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddExternalAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.ExternalAuthentication, options);
        }

        /// <summary>
        ///     Регистрирует middleware аутентификации пользователя средствами приложения.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddInternalAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.InternalAuthentication, options);
        }

        /// <summary>
        ///     Регистрирует middleware обработки запросов после аутентификации пользователя.
        /// </summary>
        /// <param name="app">Конфигуратор обработки запросов.</param>
        /// <param name="options">Обработчик запроса.</param>
        public static void AddAfterAuthenticationMiddleware(this IApplicationBuilder app, IMiddlewareOptions options)
        {
            AddUserMiddleware(HttpMiddlewareType.AfterAuthentication, options);
        }

        /// <summary>
        ///     Регистрирует middleware обработки прикладных запросов.
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

            foreach (var options in MiddlewareOptions)
            {
                foreach (var middleware in httpMiddlewares.Where(m => m.Type == options.Key))
                {
                    middleware.Configure(app, options.Value);
                }
            }
        }

        private static void AddUserMiddleware(HttpMiddlewareType key, IMiddlewareOptions value)
        {
            MiddlewareOptions[key] = value;
        }
    }
}
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
        private static readonly Dictionary<HttpMiddlewareType, Action> UserMiddlewareActions = new Dictionary<HttpMiddlewareType, Action>();

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

        public static void UseInfinniMiddlewares(this IApplicationBuilder builder, IContainerResolver resolver, IApplicationLifetime lifetime)
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

            var httpMiddlewares = resolver.Resolve<IHttpMiddleware[]>();
            var httpMiddlewareTypes = (HttpMiddlewareType[]) Enum.GetValues(typeof(HttpMiddlewareType));

            foreach (var type in httpMiddlewareTypes)
            {
                foreach (var middleware in httpMiddlewares.Where(m => m.Type == type))
                {
                    middleware.Configure(builder);
                }

                if (UserMiddlewareActions.ContainsKey(type))
                {
                    UserMiddlewareActions[type].Invoke();
                }
            }
        }

        public static void AddGlobalHandlingMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.GlobalHandling] = action;
        }

        public static void AddErrorHandlingMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.ErrorHandling] = action;
        }

        public static void AddBeforeAuthenticationMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.BeforeAuthentication] = action;
        }

        public static void AddAuthenticationBarrierMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.AuthenticationBarrier] = action;
        }

        public static void AddExternalAuthenticationMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.ExternalAuthentication] = action;
        }

        public static void AddInternalAuthenticationMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.InternalAuthentication] = action;
        }

        public static void AddAfterAuthenticationMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.AfterAuthentication] = action;
        }

        public static void AddApplicationMiddleware(this IApplicationBuilder app, Action action)
        {
            UserMiddlewareActions[HttpMiddlewareType.Application] = action;
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.AspNetCore
{
    public static class CoreServiceCollectionExtensions
    {
        /// <summary>
        /// Builds <see cref="IServiceProvider" /> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = AppOptions.Default;

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider" /> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IConfiguration configuration, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = configuration.GetSection(AppOptions.SectionName)
                .Get<AppOptions>();

            return BuildProvider(services, options);
        }

        /// <summary>
        /// Builds <see cref="IServiceProvider" /> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Application options.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
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

            // Register Autofac container itself
            IContainer autofacRootContainer = null;
            containerBuilder.RegisterInstance((Func<IContainer>) (() => autofacRootContainer));
            containerBuilder.Register(r => r.Resolve<Func<IContainer>>()()).As<IContainer>().SingleInstance();

            // ReSharper restore AccessToModifiedClosure

            containerBuilder.RegisterType<ContainerServiceRegistry>().As<IContainerServiceRegistry>().SingleInstance();
            containerBuilder.RegisterType<ServiceProviderAccessor>().As<IServiceProviderAccessor>().SingleInstance();
            containerBuilder.RegisterType<ContainerResolver>().As<IContainerResolver>().SingleInstance();

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
    }
}
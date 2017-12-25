using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InfinniPlatform.IoC;
using InfinniPlatform.Logging;
using InfinniPlatform.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for core dependencies registration.
    /// </summary>
    public static class CoreServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an application IoC-container module.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="containerModule">The application IoC-container module.</param>
        public static IServiceCollection AddContainerModule(this IServiceCollection services, IContainerModule containerModule)
        {
            return services.AddSingleton(provider => containerModule);
        }

        /// <summary>
        /// Adds customized MVC services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        public static IMvcBuilder AddMvcWithInternalServices(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc()
                .AddJsonOptions(json =>
                {
                    var settings = json.SerializerSettings;

                    settings.Formatting = Formatting.Indented;
                    settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() };
                    settings.Converters.Add(new DateJsonConverter());
                    settings.Converters.Add(new TimeJsonConverter());
                    settings.Converters.Add(new DynamicDocumentJsonConverter());
                });

            return mvcBuilder;
        }

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
        ///  Builds <see cref="IServiceProvider" /> based on registered services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="containerModules">Registered IoC-container modules.</param>
        public static IServiceProvider BuildProvider(this IServiceCollection services, IConfiguration configuration, IEnumerable<IContainerModule> containerModules = null)
        {
            var options = AppOptions.Default;
            
            configuration.GetSection(options.SectionName).Bind(options);

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

            // Register Autofac container itself
            IContainer autofacRootContainer = null;
            containerBuilder.RegisterInstance((Func<IContainer>)(() => autofacRootContainer));
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
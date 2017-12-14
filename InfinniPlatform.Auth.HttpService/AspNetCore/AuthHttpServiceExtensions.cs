using InfinniPlatform.Auth;
using InfinniPlatform.Auth.HttpService;
using InfinniPlatform.Auth.HttpService.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for setting up auth http service dependencies.
    /// </summary>
    public static class AuthHttpServiceExtensions
    {
        /// <summary>
        /// Adds auth http service dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService(this IServiceCollection services, IMvcBuilder builder)
        {
            var options = AuthHttpServiceOptions.Default;

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }

        /// <summary>
        /// Adds auth http service dependencies with settings from configuration.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService(IServiceCollection services, IMvcBuilder builder, IConfiguration configuration)
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }

        /// <summary>
        /// Adds auth http service dependencies with settings from custom options.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <param name="options">Custom options.</param>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService(IServiceCollection services, IMvcBuilder builder, AuthHttpServiceOptions options)
        {
            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<AppUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }

        /// <summary>
        /// Adds auth http service dependencies for custom user.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <typeparam name="TUser">Custom user.</typeparam>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService<TUser>(this IServiceCollection services, IMvcBuilder builder) where TUser : AppUser
        {
            var options = AuthHttpServiceOptions.Default;

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }

        /// <summary>
        /// Adds auth http service dependencies for custom user with settings from configuration. 
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <typeparam name="TUser">Custom user.</typeparam>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService<TUser>(IServiceCollection services, IMvcBuilder builder, IConfiguration configuration) where TUser : AppUser
        {
            var options = configuration.GetSection(AuthHttpServiceOptions.SectionName).Get<AuthHttpServiceOptions>();

            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }

        /// <summary>
        /// Adds auth http service dependencies for custom user with settings from custom options. 
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="builder">MVC builder.</param>
        /// <param name="options">Custom options.</param>
        /// <typeparam name="TUser">Custom user.</typeparam>
        /// <returns>Collection of registered services.</returns>
        public static IServiceCollection AddAuthHttpService<TUser>(IServiceCollection services, IMvcBuilder builder, AuthHttpServiceOptions options) where TUser : AppUser
        {
            builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new AuthControllersFeatureProvider<TUser>()));

            return services.AddSingleton(provider => new AuthHttpServiceContainerModule(options));
        }
    }
}
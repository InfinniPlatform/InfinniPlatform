using System;

using InfinniPlatform.Cache;
using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for two layers cache dependencies registration.
    /// </summary>
    public static class TwoLayerCacheExtensions
    {
        /// <summary>
        /// Register two layers cache dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services)
        {
            var options = TwoLayerCacheOptions.Default;

            return AddTwoLayerCache(services, options);
        }

        /// <summary>
        /// Register two layers cache dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="callback">Function for options customization.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services, IConfiguration configuration, Action<TwoLayerCacheOptions> callback = null)
        {
            var options = TwoLayerCacheOptions.Default;
            
            configuration.GetSection(options.SectionName).Bind(options);

            callback?.Invoke(options);

            return AddTwoLayerCache(services, options);
        }

        /// <summary>
        /// Register two layers cache dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="option">Two layers cache options.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services, TwoLayerCacheOptions option)
        {
            return services.AddSingleton(provider => new TwoLayerCacheContainerModule(option ?? TwoLayerCacheOptions.Default));
        }
    }
}
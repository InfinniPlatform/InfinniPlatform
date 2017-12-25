using InfinniPlatform.Cache;
using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for shared cache dependencies registration.
    /// </summary>
    public static class RedisSharedCacheExtensions
    {
        /// <summary>
        /// Register shared cache dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services)
        {
            var options = RedisSharedCacheOptions.Default;

            return AddRedisSharedCache(services, options);
        }

        /// <summary>
        /// Register shared cache services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(RedisSharedCacheOptions.SectionName).Get<RedisSharedCacheOptions>();

            return AddRedisSharedCache(services, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">Redis shared cache options.</param>
        /// <returns></returns>
        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services, RedisSharedCacheOptions options)
        {
            return services.AddSingleton(provider => new RedisSharedCacheContainerModule(options ?? RedisSharedCacheOptions.Default));
        }
    }
}
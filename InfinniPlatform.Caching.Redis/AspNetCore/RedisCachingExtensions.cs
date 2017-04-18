using InfinniPlatform.Cache.Redis;
using InfinniPlatform.Cache.Redis.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class RedisCachingExtensions
    {
        /// <summary>
        /// Регистрирует сервисы кэша Redis.
        /// </summary>
        /// <param name="services">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            var options = RedisCacheOptions.Default;

            return AddRedisCache(services, options);
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(RedisCacheOptions.SectionName).Get<RedisCacheOptions>();

            return AddRedisCache(services, options);
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, RedisCacheOptions redisOptions)
        {
            return services.AddSingleton(provider => new RedisCachingContainerModule(redisOptions ?? RedisCacheOptions.Default));
        }
    }
}
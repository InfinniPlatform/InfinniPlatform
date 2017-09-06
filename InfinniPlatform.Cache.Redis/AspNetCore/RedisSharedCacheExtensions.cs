using InfinniPlatform.Cache;
using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class RedisSharedCacheExtensions
    {
        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services)
        {
            var options = RedisSharedCacheOptions.Default;

            return AddRedisSharedCache(services, options);
        }

        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(RedisSharedCacheOptions.SectionName).Get<RedisSharedCacheOptions>();

            return AddRedisSharedCache(services, options);
        }

        public static IServiceCollection AddRedisSharedCache(this IServiceCollection services, RedisSharedCacheOptions options)
        {
            return services.AddSingleton(provider => new RedisSharedCacheContainerModule(options ?? RedisSharedCacheOptions.Default));
        }
    }
}
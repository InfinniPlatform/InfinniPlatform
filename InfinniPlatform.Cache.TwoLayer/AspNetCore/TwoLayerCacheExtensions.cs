using System;

using InfinniPlatform.Cache;
using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class TwoLayerCacheExtensions
    {
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services)
        {
            var options = TwoLayerCacheOptions.Default;

            return AddTwoLayerCache(services, options);
        }

        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services, IConfigurationRoot configuration, Action<TwoLayerCacheOptions> callback = null)
        {
            var options = configuration.GetSection(TwoLayerCacheOptions.SectionName).Get<TwoLayerCacheOptions>();

            callback?.Invoke(options);

            return AddTwoLayerCache(services, options);
        }

        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services, TwoLayerCacheOptions option)
        {
            return services.AddSingleton(provider => new TwoLayerCacheContainerModule(option ?? TwoLayerCacheOptions.Default));
        }
    }
}
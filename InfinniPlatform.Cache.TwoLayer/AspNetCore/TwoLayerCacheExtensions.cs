using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class TwoLayerCacheExtensions
    {
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new TwoLayerCacheContainerModule());
        }
    }
}
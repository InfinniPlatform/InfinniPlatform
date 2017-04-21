using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class TwoLayerCacheExtensions
    {
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new TwoLayerCacheContainerModule());
        }
    }
}
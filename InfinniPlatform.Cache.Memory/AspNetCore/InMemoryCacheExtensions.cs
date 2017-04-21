using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class InMemoryCacheExtensions
    {
        public static IServiceCollection AddInMemoryCache(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new InMemoryCacheContainerModule());
        }
    }
}
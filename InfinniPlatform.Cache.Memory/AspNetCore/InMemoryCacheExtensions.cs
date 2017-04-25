using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class InMemoryCacheExtensions
    {
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new InMemoryCacheContainerModule());
        }
    }
}
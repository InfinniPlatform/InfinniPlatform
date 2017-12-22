using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for in-memory cache services registration.
    /// </summary>
    public static class InMemoryCacheExtensions
    {
        /// <summary>
        /// Register services for in-memory cache. 
        /// </summary>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new InMemoryCacheContainerModule());
        }
    }
}
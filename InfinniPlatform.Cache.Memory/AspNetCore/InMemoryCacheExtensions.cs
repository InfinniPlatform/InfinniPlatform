using InfinniPlatform.Cache.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for in-memory cache dependencies registration.
    /// </summary>
    public static class InMemoryCacheExtensions
    {
        /// <summary>
        /// Register in-memory cache dependencies.
        /// </summary>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new InMemoryCacheContainerModule());
        }
    }
}
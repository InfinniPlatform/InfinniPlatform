using InfinniPlatform.Cache.Memory.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class MemoryCachingExtensions
    {
        /// <summary>
        /// Регистрирует сервисы кэша в памяти.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new MemoryCachingContainerModule());
            return serviceCollection;
        }
    }
}
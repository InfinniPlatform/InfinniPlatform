using InfinniPlatform.Caching.Memory.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class MemoryCachingExtensions
    {
        /// <summary>
        /// Регистрирует сервисы кэширования.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new MemoryCachingContainerModule());
            return serviceCollection;
        }
    }
}
using InfinniPlatform.Caching.TwoLayer.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class TwoLayerCachingExtensions
    {
        /// <summary>
        /// Регистрирует сервисы кэширования.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new TwoLayerCachingContainerModule());
            return serviceCollection;
        }
    }
}
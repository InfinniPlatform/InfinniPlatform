using InfinniPlatform.Cache.TwoLayer.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class TwoLayerCachingExtensions
    {
        /// <summary>
        /// Регистрирует сервисы двухуровневого кэша.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddTwoLayerCache(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new TwoLayerCachingContainerModule());
            return serviceCollection;
        }
    }
}
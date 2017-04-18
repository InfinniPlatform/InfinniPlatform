using InfinniPlatform.Caching.Redis.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы кэширования.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new CachingContainerModule());
            return serviceCollection;
        }
    }
}
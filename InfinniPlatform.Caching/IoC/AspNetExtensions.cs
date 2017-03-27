using InfinniPlatform.Caching.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new CachingContainerModule());
            return serviceCollection;
        }
    }
}
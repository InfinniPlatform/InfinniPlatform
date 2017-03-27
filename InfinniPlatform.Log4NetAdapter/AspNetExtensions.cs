using InfinniPlatform.Log4NetAdapter;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddLog4NetAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new LoggingContainerModule());

            return serviceCollection;
        }
    }
}
using InfinniPlatform.Logging.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class Log4NetExtensions
    {
        public static IServiceCollection AddLog4NetLogging(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new Log4NetContainerModule());
        }
    }
}
using InfinniPlatform.Log4NetAdapter;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы для log4net.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddLog4NetAdapter(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new LoggingContainerModule());

            return serviceCollection;
        }
    }
}
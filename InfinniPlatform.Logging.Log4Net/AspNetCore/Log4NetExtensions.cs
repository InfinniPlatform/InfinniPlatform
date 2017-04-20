using InfinniPlatform.Logging.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class Log4NetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы для log4net.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddLog4NetAdapter(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new Log4NetContainerModule());
        }
    }
}
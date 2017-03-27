using InfinniPlatform.Scheduler.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        /// <summary>
        /// Регистрирует сервисы планировщика заданий.
        /// </summary>
        /// <param name="serviceCollection">Коллекция зарегистрированных сервисов.</param>
        public static IServiceCollection AddScheduler(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new SchedulerContainerModule());
            return serviceCollection;
        }
    }
}
using InfinniPlatform.Scheduler.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class SchedulerHttpServiceExtensions
    {
        public static IServiceCollection AddSchedulerHttpService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new SchedulerHttpServiceContainerModule());
        }
    }
}
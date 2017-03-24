using InfinniPlatform.Scheduler.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddInfScheduler(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new SchedulerContainerModule());
            return serviceCollection;
        }
    }
}
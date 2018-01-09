using InfinniPlatform.Scheduler.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for scheduler http service dependencies registration.
    /// </summary>
    public static class SchedulerHttpServiceExtensions
    {
        /// <summary>
        /// Register scheduler http service dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddSchedulerHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new SchedulerHttpServiceContainerModule());
        }
    }
}
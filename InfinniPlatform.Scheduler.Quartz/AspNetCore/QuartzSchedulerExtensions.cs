using System;

using InfinniPlatform.Scheduler;
using InfinniPlatform.Scheduler.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for Quartz scheduler dependencies registration.
    /// </summary>
    public static class QuartzSchedulerExtensions
    {
        /// <summary>
        /// Register Quartz scheduler dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services)
        {
            var options = QuartzSchedulerOptions.Default;

            return AddQuartzScheduler(services, options);
        }

        /// <summary>
        /// Register Quartz scheduler dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <param name="callback">Function for options customization.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, IConfiguration configuration, Action<QuartzSchedulerOptions> callback = null)
        {
            var options = configuration.GetSection(QuartzSchedulerOptions.SectionName).Get<QuartzSchedulerOptions>();

            callback?.Invoke(options);

            return AddQuartzScheduler(services, options);
        }

        /// <summary>
        /// Register Quartz scheduler dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">Quartz scheduler options.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, QuartzSchedulerOptions options)
        {
            return services.AddSingleton(provider => new QuartzSchedulerContainerModule(options ?? QuartzSchedulerOptions.Default));
        }
    }
}
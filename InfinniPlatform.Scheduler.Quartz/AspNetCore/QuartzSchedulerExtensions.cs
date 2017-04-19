using InfinniPlatform.Scheduler;
using InfinniPlatform.Scheduler.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class QuartzSchedulerExtensions
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services)
        {
            var options = QuartzSchedulerOptions.Default;

            return AddQuartzScheduler(services, options);
        }

        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(QuartzSchedulerOptions.SectionName).Get<QuartzSchedulerOptions>();

            return AddQuartzScheduler(services, options);
        }

        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, QuartzSchedulerOptions options)
        {
            return services.AddSingleton(provider => new QuartzSchedulerContainerModule(options ?? QuartzSchedulerOptions.Default));
        }
    }
}
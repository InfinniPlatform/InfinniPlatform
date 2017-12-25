using InfinniPlatform.Heartbeat.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for heartbeat http service dependencies registration.
    /// </summary>
    public static class HeartbeatHttpServiceExtensions
    {
        /// <summary>
        /// Register heartbeat http service dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddHeartbeatHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new HeartbeatHttpServiceContainerModule());
        }
    }
}
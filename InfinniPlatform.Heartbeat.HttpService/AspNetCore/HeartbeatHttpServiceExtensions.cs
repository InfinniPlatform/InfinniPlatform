using InfinniPlatform.Heartbeat.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class HeartbeatHttpServiceExtensions
    {
        public static IServiceCollection AddHeartbeatHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new HeartbeatHttpServiceContainerModule());
        }
    }
}
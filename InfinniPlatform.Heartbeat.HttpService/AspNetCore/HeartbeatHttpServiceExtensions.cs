using InfinniPlatform.Heartbeat.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class HeartbeatHttpServiceExtensions
    {
        public static IServiceCollection AddHeartbeatHttpService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new HeartbeatHttpServiceContainerModule());
        }
    }
}
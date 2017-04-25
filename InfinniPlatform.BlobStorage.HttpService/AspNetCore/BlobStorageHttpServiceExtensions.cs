using InfinniPlatform.BlobStorage.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class BlobStorageHttpServiceExtensions
    {
        public static IServiceCollection AddBlobStorageHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new BlobStorageHttpServiceContainerModule());
        }
    }
}
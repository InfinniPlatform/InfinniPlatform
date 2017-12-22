using InfinniPlatform.BlobStorage.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for blob storage http service services registration.
    /// </summary>
    public static class BlobStorageHttpServiceExtensions
    {
        /// <summary>
        /// Register blob storage http service services.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        public static IServiceCollection AddBlobStorageHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new BlobStorageHttpServiceContainerModule());
        }
    }
}
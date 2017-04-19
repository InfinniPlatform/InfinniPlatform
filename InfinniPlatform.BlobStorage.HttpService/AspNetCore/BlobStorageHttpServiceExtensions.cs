using InfinniPlatform.BlobStorage.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class BlobStorageHttpServiceExtensions
    {
        public static IServiceCollection AddBlobStorageHttpService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new BlobStorageHttpServiceContainerModule());
        }
    }
}
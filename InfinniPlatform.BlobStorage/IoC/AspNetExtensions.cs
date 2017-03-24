using InfinniPlatform.BlobStorage.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddInfBlobStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new BlobStorageContainerModule());
            return serviceCollection;
        }
    }
}
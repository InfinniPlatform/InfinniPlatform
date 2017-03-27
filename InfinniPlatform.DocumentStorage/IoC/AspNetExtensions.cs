using InfinniPlatform.DocumentStorage.IoC;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.Extensions
{
    public static class AspNetExtensions
    {
        public static IServiceCollection AddDocumentStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider => new DocumentStorageContainerModule());
            return serviceCollection;
        }
    }
}
using InfinniPlatform.DocumentStorage.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class DocumentStorageHttpServiceExtensions
    {
        public static IServiceCollection AddDocumentStorageHttpService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton(provider => new DocumentStorageHttpServiceContainerModule());
        }
    }
}
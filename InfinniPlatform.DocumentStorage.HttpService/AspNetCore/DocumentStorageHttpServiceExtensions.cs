using InfinniPlatform.DocumentStorage.IoC;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for document storage http service dependencies registration.
    /// </summary>
    public static class DocumentStorageHttpServiceExtensions
    {
        /// <summary>
        /// Register document storage http service dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddDocumentStorageHttpService(this IServiceCollection services)
        {
            return services.AddSingleton(provider => new DocumentStorageHttpServiceContainerModule());
        }
    }
}
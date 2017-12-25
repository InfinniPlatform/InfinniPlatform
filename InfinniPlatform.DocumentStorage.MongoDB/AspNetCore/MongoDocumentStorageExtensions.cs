using InfinniPlatform.DocumentStorage;
using InfinniPlatform.DocumentStorage.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for MongoDB document storage dependencies registration.
    /// </summary>
    public static class MongoDocumentStorageExtensions
    {
        /// <summary>
        /// Register MongoDB document storage dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services)
        {
            var options = MongoDocumentStorageOptions.Default;

            return AddMongoDocumentStorage(services, options);
        }

        /// <summary>
        /// Register MongoDB document storage dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var options = MongoDocumentStorageOptions.Default;
            
            configuration.GetSection(options.SectionName).Bind(options);

            return AddMongoDocumentStorage(services, options);
        }

        /// <summary>
        /// Register MongoDB document storage dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">MongoDB document storage options.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services, MongoDocumentStorageOptions options)
        {
            return services.AddSingleton(provider => new MongoDocumentStorageContainerModule(options ?? MongoDocumentStorageOptions.Default));
        }
    }
}
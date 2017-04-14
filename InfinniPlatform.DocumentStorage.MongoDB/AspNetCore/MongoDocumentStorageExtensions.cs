using InfinniPlatform.DocumentStorage.MongoDB;
using InfinniPlatform.DocumentStorage.MongoDB.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class MongoDocumentStorageExtensions
    {
        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services)
        {
            var options = MongoDocumentStorageOptions.Default;

            return AddMongoDocumentStorage(services, options);
        }

        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(MongoDocumentStorageOptions.SectionName).Get<MongoDocumentStorageOptions>();

            return AddMongoDocumentStorage(services, options);
        }

        public static IServiceCollection AddMongoDocumentStorage(this IServiceCollection services, MongoDocumentStorageOptions options)
        {
            return services.AddSingleton(provider => new MongoDocumentStorageContainerModule(options ?? MongoDocumentStorageOptions.Default));
        }
    }
}
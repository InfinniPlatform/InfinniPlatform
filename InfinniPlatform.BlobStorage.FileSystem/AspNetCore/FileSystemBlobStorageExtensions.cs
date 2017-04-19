using InfinniPlatform.BlobStorage;
using InfinniPlatform.BlobStorage.IoC;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class FileSystemBlobStorageExtensions
    {
        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services)
        {
            var options = FileSystemBlobStorageOptions.Default;

            return AddFileSystemBlobStorage(services, options);
        }

        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(FileSystemBlobStorageOptions.SectionName).Get<FileSystemBlobStorageOptions>();

            return AddFileSystemBlobStorage(services, options);
        }

        public static IServiceCollection AddFileSystemBlobStorage(this IServiceCollection services, FileSystemBlobStorageOptions options)
        {
            return services.AddSingleton(provider => new FileSystemBlobStorageContainerModule(options ?? FileSystemBlobStorageOptions.Default));
        }
    }
}
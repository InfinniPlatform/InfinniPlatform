using InfinniPlatform.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.BlobStorage" />.
    /// </summary>
    public class FileSystemBlobStorageContainerModule : IContainerModule
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FileSystemBlobStorageContainerModule" />.
        /// </summary>
        /// <param name="options">File system blob storage options.</param>
        public FileSystemBlobStorageContainerModule(FileSystemBlobStorageOptions options)
        {
            _options = options ?? new FileSystemBlobStorageOptions();
        }

        private readonly FileSystemBlobStorageOptions _options;

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            builder.RegisterType<FileSystemBlobStorage>().As<IBlobStorage>().SingleInstance();
        }
    }
}
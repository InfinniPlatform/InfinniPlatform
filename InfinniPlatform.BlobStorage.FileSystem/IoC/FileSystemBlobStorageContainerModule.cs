using InfinniPlatform.IoC;

namespace InfinniPlatform.BlobStorage.IoC
{
    public class FileSystemBlobStorageContainerModule : IContainerModule
    {
        public FileSystemBlobStorageContainerModule(FileSystemBlobStorageOptions options)
        {
            _options = options ?? new FileSystemBlobStorageOptions();
        }

        private readonly FileSystemBlobStorageOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            builder.RegisterType<FileSystemBlobStorage>().As<IBlobStorage>().SingleInstance();
        }
    }
}
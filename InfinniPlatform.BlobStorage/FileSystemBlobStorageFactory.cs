using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Фабрика для создания сервисов по работе с BLOB на базе файловой системы.
    /// </summary>
    public sealed class FileSystemBlobStorageFactory : IBlobStorageFactory
    {
        public FileSystemBlobStorageFactory(IAppConfiguration appConfiguration, IPerformanceLog performanceLog)
        {
            var settings = appConfiguration.GetSection<FileSystemBlobStorageSettings>(FileSystemBlobStorageSettings.SectionName);

            _blobStorage = new FileSystemBlobStorage(settings.BaseDirectory, performanceLog);
        }


        private readonly FileSystemBlobStorage _blobStorage;


        public IBlobStorage CreateBlobStorage()
        {
            return _blobStorage;
        }
    }
}
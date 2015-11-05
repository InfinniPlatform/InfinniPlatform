using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment.Binary;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Фабрика для создания сервисов по работе с BLOB на базе файловой системы.
    /// </summary>
    public sealed class FileSystemBlobStorageFactory : IBlobStorageFactory
    {
        public FileSystemBlobStorageFactory()
        {
            var baseDirectory = AppSettings.GetValue("BlobStorageBaseDirectory", "BlobStorage");

            _blobStorage = new FileSystemBlobStorage(baseDirectory);
        }


        private readonly FileSystemBlobStorage _blobStorage;


        public IBlobStorage CreateBlobStorage()
        {
            return _blobStorage;
        }
    }
}
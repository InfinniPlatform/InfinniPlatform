namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Настройки сервиса для работы хранилищем BLOB.
    /// </summary>
    internal sealed class FileSystemBlobStorageSettings
    {
        public const string SectionName = "blobStorage";


        public FileSystemBlobStorageSettings()
        {
            BaseDirectory = "BlobStorage";
        }


        /// <summary>
        /// Базовый каталог для хранения файлов системы.
        /// </summary>
        public string BaseDirectory { get; set; }
    }
}
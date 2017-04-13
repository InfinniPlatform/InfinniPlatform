namespace InfinniPlatform.BlobStorage.FileSystem
{
    /// <summary>
    /// Настройки сервиса для работы хранилищем BLOB.
    /// </summary>
    public class FileSystemBlobStorageOptions
    {
        public const string SectionName = "fileSystemBlobStorage";

        public const string DefaultBaseDirectory = "BlobStorage";

        public static readonly FileSystemBlobStorageOptions Default = new FileSystemBlobStorageOptions();


        public FileSystemBlobStorageOptions()
        {
            BaseDirectory = DefaultBaseDirectory;
        }


        /// <summary>
        /// Базовый каталог для хранения файлов системы.
        /// </summary>
        public string BaseDirectory { get; set; }
    }
}
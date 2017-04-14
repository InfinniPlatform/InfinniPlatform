namespace InfinniPlatform.BlobStorage.FileSystem
{
    /// <summary>
    /// Настройки хранилища BLOB в файловой системе.
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
namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// File system blob storage configuration options.
    /// </summary>
    public class FileSystemBlobStorageOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "fileSystemBlobStorage";

        /// <summary>
        /// Default directory for blob storage.
        /// </summary>
        public const string DefaultBaseDirectory = "BlobStorage";

        /// <summary>
        /// Default instance of <see cref="FileSystemBlobStorageOptions" />.
        /// </summary>
        public static readonly FileSystemBlobStorageOptions Default = new FileSystemBlobStorageOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="FileSystemBlobStorageOptions" />.
        /// </summary>
        public FileSystemBlobStorageOptions()
        {
            BaseDirectory = DefaultBaseDirectory;
        }


        /// <summary>
        /// Base directory for file system blob storage.
        /// </summary>
        public string BaseDirectory { get; set; }
    }
}
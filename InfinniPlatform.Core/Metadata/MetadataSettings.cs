namespace InfinniPlatform.Core.Metadata
{
    public class MetadataSettings
    {
        /// <summary>
        /// Настройки хранения метаданных.
        /// </summary>
        public const string SectionName = "metadata";

        public MetadataSettings()
        {
            ContentDirectory = "content";
            ViewsDirectoryPath = "content/Views";
        }

        /// <summary>
        /// Каталог с файлами метаданных.
        /// </summary>
        public string ContentDirectory { get; set; }

        /// <summary>
        /// Путь до каталога с файлами Razor-представлений.
        /// </summary>
        public string ViewsDirectoryPath { get; set; }
    }
}
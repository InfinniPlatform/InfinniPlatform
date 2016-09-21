namespace InfinniPlatform.Core.Metadata
{
    /// <summary>
    /// Настройки хранения метаданных.
    /// </summary>
    public class MetadataSettings
    {
        public const string SectionName = "metadata";

        public MetadataSettings()
        {
            DocumentsPath = "content/metadata/Documents";
            PrintViewsPath = "content/metadata/PrintViews";
        }

        /// <summary>
        /// Путь до подкаталога с файлами метаданных документов.
        /// </summary>
        public string DocumentsPath { get; set; }

        /// <summary>
        /// Путь до подкаталога с файлами метаданных печатных представлений.
        /// </summary>
        public string PrintViewsPath { get; set; }
    }
}
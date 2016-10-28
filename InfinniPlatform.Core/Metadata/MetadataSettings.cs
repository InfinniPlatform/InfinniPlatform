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
        }

        /// <summary>
        /// Путь до подкаталога с файлами метаданных документов.
        /// </summary>
        public string DocumentsPath { get; set; }
    }
}
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
            ViewsPath = "metadata/Views";
            DocumentsPath = "metadata/Documents";
            PrintViewsPath = "metadata/PrintViews";
            RazorViewsPath = "Views";
        }

        /// <summary>
        /// Корневой каталог со статическими файлами.
        /// </summary>
        public string ContentDirectory { get; set; }

        /// <summary>
        /// Путь до подкаталога с файлами метаданных представлений.
        /// </summary>
        public string ViewsPath { get; set; }

        /// <summary>
        /// Путь до подкаталога  с файлами метаданных документов.
        /// </summary>
        public string DocumentsPath { get; set; }

        /// <summary>
        /// Путь до подкаталога  с файлами метаданных печатных представлений.
        /// </summary>
        public string PrintViewsPath { get; set; }

        /// <summary>
        /// Путь до каталога с файлами Razor-представлений.
        /// </summary>
        public string RazorViewsPath { get; set; }
    }
}
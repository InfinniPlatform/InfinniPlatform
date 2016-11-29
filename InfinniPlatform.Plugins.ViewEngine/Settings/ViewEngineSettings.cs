namespace InfinniPlatform.Plugins.ViewEngine.Settings
{
    /// <summary>
    /// Настройки хостинга статических файлов.
    /// </summary>
    public class ViewEngineSettings
    {
        public const string SectionName = "staticContent";

        /// <summary>
        /// Путь до файлов Razor-представлений (относительно рабочей папки).
        /// </summary>
        public string RazorViewsPath { get; set; }

        /// <summary>
        /// Путь до файлов Razor-представлений (относительно рабочей папки).
        /// </summary>
        public string[] EmbeddedRazorViewsAssemblies { get; set; }
    }
}
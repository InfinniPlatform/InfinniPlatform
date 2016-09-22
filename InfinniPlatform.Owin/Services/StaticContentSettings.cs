using System.Collections.Generic;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Настройки хостинга статических файлов.
    /// </summary>
    public class StaticContentSettings
    {
        public const string SectionName = "staticContent";

        public StaticContentSettings()
        {
            StaticContentMapping = new Dictionary<string, string>();
            ResourceContentMapping = new Dictionary<string, string>();
            RazorViewsPath = string.Empty;
        }

        /// <summary>
        /// Соответствие виртуальных и физических путей до статических файлов.
        /// </summary>
        public Dictionary<string, string> StaticContentMapping { get; set; }

        /// <summary>
        /// Соответствие виртуальных путей и сборок с файлами ресурсов.
        /// </summary>
        public Dictionary<string, string> ResourceContentMapping { get; set; }

        /// <summary>
        /// Путь до файлов Razor-представлений (относительно рабочей папки).
        /// </summary>
        public string RazorViewsPath { get; set; }
    }
}
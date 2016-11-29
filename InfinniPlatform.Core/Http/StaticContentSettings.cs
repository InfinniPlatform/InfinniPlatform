using System.Collections.Generic;

namespace InfinniPlatform.Core.Http
{
    /// <summary>
    /// Настройки хостинга статических файлов.
    /// </summary>
    public class StaticContentSettings
    {
        public const string SectionName = "staticContent";

        public StaticContentSettings()
        {
            StaticFilesMapping = new Dictionary<string, string>();
            EmbeddedResourceMapping = new Dictionary<string, string>();
        }

        /// <summary>
        /// Соответствие виртуальных и физических путей до статических файлов.
        /// </summary>
        public Dictionary<string, string> StaticFilesMapping { get; set; }

        /// <summary>
        /// Соответствие виртуальных путей и сборок с файлами ресурсов.
        /// </summary>
        public Dictionary<string, string> EmbeddedResourceMapping { get; set; }
    }
}
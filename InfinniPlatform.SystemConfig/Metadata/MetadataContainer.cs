using System.Collections.Generic;

namespace InfinniPlatform.SystemConfig.Metadata
{
    /// <summary>
    /// Метаданные документа.
    /// </summary>
    internal sealed class DocumentMetadata
    {
        /// <summary>
        /// Список печатных представлений.
        /// </summary>
        public IEnumerable<dynamic> PrintViews = new List<dynamic>();

        /// <summary>
        /// Список процессов.
        /// </summary>
        public IEnumerable<dynamic> Processes = new List<dynamic>();

        /// <summary>
        /// Список сценариев.
        /// </summary>
        public IEnumerable<dynamic> Scenario = new List<dynamic>();

        /// <summary>
        /// Список представлений.
        /// </summary>
        public IEnumerable<dynamic> Views = new List<dynamic>();
    }
}
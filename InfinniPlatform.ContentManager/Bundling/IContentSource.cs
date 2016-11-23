using System.Collections.Generic;

namespace InfinniPlatform.ContentManager.Bundling
{
    /// <summary>
    /// Источник статического контента для UI.
    /// </summary>
    public interface IContentSource
    {
        /// <summary>
        /// Список относительных путей до css-файлов.
        /// </summary>
        IEnumerable<string> Css();

        /// <summary>
        /// Список относительных путей до less-файлов.
        /// </summary>
        IEnumerable<string> Less();

        /// <summary>
        /// Список относительных путей до js-файлов.
        /// </summary>
        IEnumerable<string> Js();

        /// <summary>
        /// Список относительных путей до json-файлов.
        /// </summary>
        IEnumerable<string> Json();

        /// <summary>
        /// Список относительных путей до файлов ресурсов.
        /// </summary>
        IEnumerable<string> Assets();
    }
}
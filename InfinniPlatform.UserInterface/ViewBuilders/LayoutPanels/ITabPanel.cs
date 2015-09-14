using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels
{
    public interface ITabPanel : ILayoutPanel
    {
        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения выделенной страницы.
        /// </summary>
        ScriptDelegate OnSelectionChanged { get; set; }

        // HeaderLocation

        /// <summary>
        ///     Возвращает расположение закладок.
        /// </summary>
        TabHeaderLocation GetHeaderLocation();

        /// <summary>
        ///     Устанавливает расположение закладок.
        /// </summary>
        void SetHeaderLocation(TabHeaderLocation value);

        // HeaderOrientation

        /// <summary>
        ///     Возвращает ориентацию закладок.
        /// </summary>
        TabHeaderOrientation GetHeaderOrientation();

        /// <summary>
        ///     Устанавливает ориентацию закладок.
        /// </summary>
        void SetHeaderOrientation(TabHeaderOrientation value);

        // SelectedPage

        /// <summary>
        ///     Возвращает выделенную страницу.
        /// </summary>
        ITabPage GetSelectedPage();

        /// <summary>
        ///     Устанавливает выделенную страницу.
        /// </summary>
        void SetSelectedPage(ITabPage page);

        // Pages

        /// <summary>
        ///     Создает страницу.
        /// </summary>
        ITabPage CreatePage(View view);

        /// <summary>
        ///     Добавляет указанную страницу.
        /// </summary>
        void AddPage(ITabPage page);

        /// <summary>
        ///     Удаляет указанную страницу.
        /// </summary>
        void RemovePage(ITabPage page);

        /// <summary>
        ///     Возвращает страницу с указанным именем.
        /// </summary>
        ITabPage GetPage(string name);

        /// <summary>
        ///     Возвращает список страниц.
        /// </summary>
        IEnumerable<ITabPage> GetPages();
    }
}
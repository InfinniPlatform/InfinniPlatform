using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels
{
    public interface ITabPage : IElement
    {
        // Events

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление получает фокус.
        /// </summary>
        ScriptDelegate OnGotFocus { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что представление теряет фокус.
        /// </summary>
        ScriptDelegate OnLostFocus { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что страница закрывается.
        /// </summary>
        ScriptDelegate OnClosing { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает обработчик события о том, что страница закрыта.
        /// </summary>
        ScriptDelegate OnClosed { get; set; }

        // Parent

        /// <summary>
        ///     Возвращает родительскую панель закладок.
        /// </summary>
        ITabPanel GetParent();

        /// <summary>
        ///     Устанавливает родительскую панель закладок.
        /// </summary>
        void SetParent(ITabPanel value);

        // Image

        /// <summary>
        ///     Возвращает изображение заголовка страницы.
        /// </summary>
        string GetImage();

        /// <summary>
        ///     Устанавливает изображение заголовка страницы.
        /// </summary>
        void SetImage(string value);

        // CanClose

        /// <summary>
        ///     Возвращает значение, определяющее, разрешено ли закрытие страницы.
        /// </summary>
        bool GetCanClose();

        /// <summary>
        ///     Устанавливает значение, определяющее, разрешено ли закрытие страницы.
        /// </summary>
        void SetCanClose(bool value);

        // LayoutPanel

        /// <summary>
        ///     Возвращает контейнер элементов страницы.
        /// </summary>
        ILayoutPanel GetLayoutPanel();

        /// <summary>
        ///     Устанавливает контейнер элементов страницы.
        /// </summary>
        void SetLayoutPanel(ILayoutPanel layoutPanel);

        // Close

        /// <summary>
        ///     Закрывает страницу.
        /// </summary>
        bool Close(bool force = false);
    }
}
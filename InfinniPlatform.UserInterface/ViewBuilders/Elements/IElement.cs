using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Elements
{
    /// <summary>
    ///     Элемент представления.
    /// </summary>
    public interface IElement : IViewChild
    {
        /// <summary>
        ///     Возвращает наименование элемента.
        /// </summary>
        string GetName();

        /// <summary>
        ///     Устанавливает наименование элемента.
        /// </summary>
        void SetName(string value);

        /// <summary>
        ///     Возвращает текст заголовка элемента.
        /// </summary>
        string GetText();

        /// <summary>
        ///     Устанавливает текст заголовка элемента.
        /// </summary>
        void SetText(string value);

        /// <summary>
        ///     Возвращает текст подсказки элемента.
        /// </summary>
        string GetToolTip();

        /// <summary>
        ///     Устанавливает текст подсказки элемента.
        /// </summary>
        void SetToolTip(string value);

        /// <summary>
        ///     Возвращает значение, определяющее, возможен ли доступ к элементу.
        /// </summary>
        bool GetEnabled();

        /// <summary>
        ///     Устанавливает значение, определяющее, возможен ли доступ к элементу.
        /// </summary>
        void SetEnabled(bool value);

        /// <summary>
        ///     Возвращает значение, определяющее, отображается ли элемент в интерфейсе.
        /// </summary>
        bool GetVisible();

        /// <summary>
        ///     Устанавливает значение, определяющее, отображается ли элемент в интерфейсе.
        /// </summary>
        void SetVisible(bool value);

        /// <summary>
        ///     Возвращает вертикальное выравнивание в родительском элементе.
        /// </summary>
        ElementVerticalAlignment GetVerticalAlignment();

        /// <summary>
        ///     Устанавливает вертикальное выравнивание в родительском элементе.
        /// </summary>
        void SetVerticalAlignment(ElementVerticalAlignment value);

        /// <summary>
        ///     Возвращает горизонтальное выравнивание в родительском элементе.
        /// </summary>
        ElementHorizontalAlignment GetHorizontalAlignment();

        /// <summary>
        ///     Устанавливает горизонтальное выравнивание в родительском элементе.
        /// </summary>
        void SetHorizontalAlignment(ElementHorizontalAlignment value);

        /// <summary>
        ///     Возвращает список дочерних элементов.
        /// </summary>
        IEnumerable<IElement> GetChildElements();

        /// <summary>
        ///     Вызывает проверку состояния элемента.
        /// </summary>
        bool Validate();

        /// <summary>
        ///     Устанавливает фокус ввода на элемент.
        /// </summary>
        void Focus();

        /// <summary>
        ///     Возврщает реализацию элемента представления.
        /// </summary>
        object GetControl();
    }
}
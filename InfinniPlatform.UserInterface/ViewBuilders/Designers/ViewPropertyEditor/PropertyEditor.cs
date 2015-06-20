using System.Windows.Media;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor
{
    /// <summary>
    ///     Редактор свойства.
    /// </summary>
    public sealed class PropertyEditor
    {
        /// <summary>
        ///     Наименование редактора.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Изображение редактора.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        ///     Имя свойства.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///     Тип свойства.
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        ///     Тип значения свойства.
        /// </summary>
        public string PropertyValueType { get; set; }

        /// <summary>
        ///     Представление для редактирования свойства.
        /// </summary>
        public LinkView EditView { get; set; }
    }
}
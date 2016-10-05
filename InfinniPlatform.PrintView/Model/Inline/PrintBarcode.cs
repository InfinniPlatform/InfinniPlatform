using InfinniPlatform.PrintView.Model.Format;

namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Элемент для создания штрих-кода.
    /// </summary>
    public abstract class PrintBarcode : PrintInline
    {
        /// <summary>
        /// Показывать ли текст в штрих-коде.
        /// </summary>
        public bool? ShowText { get; set; }

        /// <summary>
        /// Текст для кодирования.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Формат отображения значения источника данных.
        /// </summary>
        public ValueFormat SourceFormat { get; set; }

        /// <summary>
        /// Поворот изображения штрих-кода.
        /// </summary>
        public PrintImageRotation? Rotation { get; set; }

        /// <summary>
        /// Размеры изображения штрих-кода.
        /// </summary>
        public PrintSize Size { get; set; }
    }
}
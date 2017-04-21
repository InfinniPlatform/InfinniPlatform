using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Настройки шрифта.
    /// </summary>
    /// <remarks>
    /// Предполагается поддержка только OpenType шрифтов.
    /// </remarks>
    [Serializable]
    public class PrintFont
    {
        /// <summary>
        /// Семейство шрифта.
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        public double? Size { get; set; }

        /// <summary>
        /// Единица измерения размера шрифта.
        /// </summary>
        public PrintSizeUnit? SizeUnit { get; set; }

        /// <summary>
        /// Стиль шрифта.
        /// </summary>
        public PrintFontStyle? Style { get; set; }

        /// <summary>
        /// Степень растягивания шрифта по горизонтали.
        /// </summary>
        public PrintFontStretch? Stretch { get; set; }

        /// <summary>
        /// Насыщенность шрифта.
        /// </summary>
        public PrintFontWeight? Weight { get; set; }

        /// <summary>
        /// Вертикальное выравнивание шрифта.
        /// </summary>
        public PrintFontVariant? Variant { get; set; }
    }
}
using System;

using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Format;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Model.Defaults
{
    /// <summary>
    /// Значения по умолчанию.
    /// </summary>
    public static class PrintViewDefaults
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedMember.Global


        /// <summary>
        /// Цвета.
        /// </summary>
        public static class Colors
        {
            /// <summary>
            /// Черный.
            /// </summary>
            public const string Black = "Black";

            /// <summary>
            /// Белый.
            /// </summary>
            public const string White = "White";

            /// <summary>
            /// Красный.
            /// </summary>
            public const string Red = "Red";

            /// <summary>
            /// Зеленый.
            /// </summary>
            public const string Green = "Green";

            /// <summary>
            /// Синий.
            /// </summary>
            public const string Blue = "Blue";

            /// <summary>
            /// Синий.
            /// </summary>
            public const string Yellow = "Yellow";

            /// <summary>
            /// Прозрачный.
            /// </summary>
            public const string Transparent = "Transparent";
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="ValueFormat"/>.
        /// </summary>
        public static class Formats
        {
            /// <summary>
            /// Текст для отображения истинного значения.
            /// </summary>
            public static string TrueText { get; set; } = true.ToString();

            /// <summary>
            /// Текст для отображения ложного значения.
            /// </summary>
            public static string FalseText { get; set; } = false.ToString();

            /// <summary>
            /// Строка форматирования даты и времени.
            /// </summary>
            public static string DateTimeFormat { get; set; } = "G";

            /// <summary>
            /// Строка форматирования числового значения.
            /// </summary>
            public static string NumberFormat { get; set; } = "n";
        }


        /// <summary>
        /// Размеры страниц.
        /// </summary>
        public static class PageSizes
        {
            /// <summary>
            /// Формат A4.
            /// </summary>
            public static readonly PrintSize A4 = new PrintSize(width: 210, height: 297, sizeUnit: PrintSizeUnit.Mm);
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintFont"/>.
        /// </summary>
        public static class FontSettings
        {
            /// <summary>
            /// Семейство шрифта.
            /// </summary>
            public static string Family { get; set; } = "Arial";

            /// <summary>
            /// Размер шрифта.
            /// </summary>
            public static double Size { get; set; } = 12;

            /// <summary>
            /// Единица измерения размера шрифта.
            /// </summary>
            public static PrintSizeUnit SizeUnit { get; set; } = PrintSizeUnit.Pt;

            /// <summary>
            /// Стиль шрифта.
            /// </summary>
            public static PrintFontStyle Style { get; set; } = PrintFontStyle.Normal;

            /// <summary>
            /// Степень растягивания шрифта по горизонтали.
            /// </summary>
            public static PrintFontStretch Stretch { get; set; } = PrintFontStretch.Normal;

            /// <summary>
            /// Насыщенность шрифта.
            /// </summary>
            public static PrintFontWeight Weight { get; set; } = PrintFontWeight.Normal;

            /// <summary>
            /// Вертикальное выравнивание шрифта.
            /// </summary>
            public static PrintFontVariant Variant { get; set; } = PrintFontVariant.Normal;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintElement"/>.
        /// </summary>
        public static class Element
        {
            /// <summary>
            /// Настройки шрифта.
            /// </summary>
            public static PrintFont Font { get; set; } = new PrintFont
            {
                Family = FontSettings.Family,
                Size = FontSettings.Size,
                SizeUnit = FontSettings.SizeUnit,
                Style = FontSettings.Style,
                Stretch = FontSettings.Stretch,
                Weight = FontSettings.Weight,
                Variant = FontSettings.Variant
            };

            /// <summary>
            /// Цвет содержимого.
            /// </summary>
            public static string Foreground { get; set; } = Colors.Black;

            /// <summary>
            /// Цвет фона содержимого.
            /// </summary>
            public static string Background { get; set; } = Colors.Transparent;

            /// <summary>
            /// Регистр символов текста.
            /// </summary>
            public static PrintTextCase TextCase { get; set; } = PrintTextCase.Normal;

            /// <summary>
            /// Видимость элемента.
            /// </summary>
            public static PrintVisibility Visibility { get; set; } = PrintVisibility.Source;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintBlock"/>.
        /// </summary>
        public static class Block
        {
            /// <summary>
            /// Границы элемента.
            /// </summary>
            public static PrintBorder Border { get; set; } = null;

            /// <summary>
            /// Отступ от края элемента до родительского элемента.
            /// </summary>
            public static PrintThickness Margin { get; set; } = PrintThickness.Zero;

            /// <summary>
            /// Отступ от края элемента до содержимого элемента.
            /// </summary>
            public static PrintThickness Padding { get; set; } = PrintThickness.Zero;

            /// <summary>
            /// Горизонтальное выравнивание текста элемента.
            /// </summary>
            public static PrintTextAlignment TextAlignment { get; set; } = PrintTextAlignment.Left;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintInline"/>.
        /// </summary>
        public static class Inline
        {
            /// <summary>
            /// Оформление текста.
            /// </summary>
            public static PrintTextDecoration TextDecoration { get; set; } = PrintTextDecoration.Normal;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintLine"/>.
        /// </summary>
        public static class Line
        {
            /// <summary>
            /// Граница линии по умолчанию.
            /// </summary>
            public static PrintBorder Border { get; set; } = new PrintBorder
            {
                Color = Element.Foreground,
                Thickness = new PrintThickness(0, 0, 0, 1, FontSettings.SizeUnit)
            };
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintList"/>.
        /// </summary>
        public static class List
        {
            /// <summary>
            /// Индекс первого элемента списка.
            /// </summary>
            public static int StartIndex { get; set; } = 1;

            /// <summary>
            /// Стиль маркера элементов списка.
            /// </summary>
            public static PrintListMarkerStyle MarkerStyle { get; set; } = PrintListMarkerStyle.Disc;

            /// <summary>
            /// Отступ содержимого элемента от края маркера.
            /// </summary>
            public static double MarkerOffsetSize { get; set; } = 0;

            /// <summary>
            /// Единица измерения отступа содержимого элемента от края маркера.
            /// </summary>
            public static PrintSizeUnit MarkerOffsetSizeUnit { get; set; } = FontSettings.SizeUnit;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintTable"/>.
        /// </summary>
        public static class Table
        {
            /// <summary>
            /// Показывать ли заголовок таблицы.
            /// </summary>
            public static bool ShowHeader { get; set; } = true;

            /// <summary>
            /// Границы таблицы.
            /// </summary>
            public static PrintBorder Border { get; set; } = new PrintBorder
            {
                Color = Element.Foreground,
                Thickness = new PrintThickness(1, 1, 0, 0, FontSettings.SizeUnit)
            };
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintTableColumn"/>.
        /// </summary>
        public static class TableColumn
        {
            /// <summary>
            /// Ширина столбца.
            /// </summary>
            public static double? Size { get; set; } = null;

            /// <summary>
            /// Единица измерения ширины столбца.
            /// </summary>
            public static PrintSizeUnit SizeUnit { get; set; } = FontSettings.SizeUnit;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintTableRow"/>.
        /// </summary>
        public static class TableRow
        {
            /// <summary>
            /// Настройки шрифта.
            /// </summary>
            public static PrintFont Font { get; set; } = Element.Font;

            /// <summary>
            /// Цвет содержимого.
            /// </summary>
            public static string Foreground { get; set; } = Element.Foreground;

            /// <summary>
            /// Цвет фона содержимого.
            /// </summary>
            public static string Background { get; set; } = Element.Background;

            /// <summary>
            /// Регистр символов текста.
            /// </summary>
            public static PrintTextCase TextCase { get; set; } = Element.TextCase;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintTableCell"/>.
        /// </summary>
        public static class TableCell
        {
            /// <summary>
            /// Настройки шрифта.
            /// </summary>
            public static PrintFont Font { get; set; } = Element.Font;

            /// <summary>
            /// Цвет содержимого.
            /// </summary>
            public static string Foreground { get; set; } = Element.Foreground;

            /// <summary>
            /// Цвет фона содержимого.
            /// </summary>
            public static string Background { get; set; } = Element.Background;

            /// <summary>
            /// Регистр символов текста.
            /// </summary>
            public static PrintTextCase TextCase { get; set; } = Element.TextCase;

            /// <summary>
            /// Границы элемента.
            /// </summary>
            public static PrintBorder Border { get; set; } = new PrintBorder
            {
                Color = Element.Foreground,
                Thickness = new PrintThickness(0, 0, 1, 1, FontSettings.SizeUnit)
            };

            /// <summary>
            /// Отступ от края элемента до содержимого элемента.
            /// </summary>
            public static PrintThickness Padding { get; set; } = Block.Padding;

            /// <summary>
            /// Горизонтальное выравнивание текста элемента.
            /// </summary>
            public static PrintTextAlignment TextAlignment { get; set; } = Block.TextAlignment;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintImage"/>.
        /// </summary>
        public static class Image
        {
            /// <summary>
            /// Поворот изображения.
            /// </summary>
            public static PrintImageRotation Rotation { get; set; } = PrintImageRotation.Rotate0;

            /// <summary>
            /// Растягивание изображения.
            /// </summary>
            public static PrintImageStretch Stretch { get; set; } = PrintImageStretch.None;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintBarcodeEan13"/>.
        /// </summary>
        public static class BarcodeEan13
        {
            /// <summary>
            /// Текст в случае отсутствия значения.
            /// </summary>
            public static string NullText { get; set; } = "0";

            /// <summary>
            /// Показывать ли текст в штрих-коде.
            /// </summary>
            public static bool ShowText { get; set; } = true;

            /// <summary>
            /// Поворот изображения штрих-кода.
            /// </summary>
            public static PrintImageRotation Rotation { get; set; } = PrintImageRotation.Rotate0;

            /// <summary>
            /// Размеры изображения штрих-кода.
            /// </summary>
            public static Func<PrintSize> Size { get; set; } = () => new PrintSize(width: 128, height: 64, sizeUnit: PrintSizeUnit.Px);

            /// <summary>
            /// Автоматически рассчитывать контрольную сумму.
            /// </summary>
            public static bool CalcCheckSum { get; set; } = true;

            /// <summary>
            /// Относительная ширина штрихов в штрих-коде.
            /// </summary>
            public static double? WideBarRatio { get; set; } = 2;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintBarcodeQr"/>.
        /// </summary>
        public static class BarcodeQr
        {
            /// <summary>
            /// Текст в случае отсутствия значения.
            /// </summary>
            public static string NullText { get; set; } = "0";

            /// <summary>
            /// Показывать ли текст в штрих-коде.
            /// </summary>
            public static bool ShowText { get; set; } = true;

            /// <summary>
            /// Поворот изображения штрих-кода.
            /// </summary>
            public static PrintImageRotation Rotation { get; set; } = PrintImageRotation.Rotate0;

            /// <summary>
            /// Размеры изображения штрих-кода.
            /// </summary>
            public static Func<PrintSize> Size { get; set; } = () => new PrintSize(width: 116, height: 116, sizeUnit: PrintSizeUnit.Px);

            /// <summary>
            /// Уровень защиты от ошибок.
            /// </summary>
            public static PrintBarcodeQrErrorCorrection ErrorCorrection { get; set; } = PrintBarcodeQrErrorCorrection.Low;
        }


        /// <summary>
        /// Настройки по умолчанию для <see cref="PrintDocument"/>.
        /// </summary>
        public static class Document
        {
            /// <summary>
            /// Размеры страницы.
            /// </summary>
            public static PrintSize PageSize { get; set; } = PageSizes.A4;

            /// <summary>
            /// Отступ от края страницы до содержимого страницы.
            /// </summary>
            public static PrintThickness PagePadding { get; set; } = new PrintThickness(10, PrintSizeUnit.Mm);
        }


        // ReSharper restore UnusedMember.Global
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    }
}
using System;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Block
{
    /// <summary>
    /// Элемент для создания ячейки таблицы.
    /// </summary>
    [Serializable]
    public class PrintTableCell : PrintNamedItem
    {
        /// <summary>
        /// Наименование стиля.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Настройки шрифта.
        /// </summary>
        public PrintFont Font { get; set; }

        /// <summary>
        /// Цвет содержимого.
        /// </summary>
        public string Foreground { get; set; }

        /// <summary>
        /// Цвет фона содержимого.
        /// </summary>
        public string Background { get; set; }

        /// <summary>
        /// Регистр символов текста.
        /// </summary>
        public PrintTextCase? TextCase { get; set; }

        /// <summary>
        /// Границы элемента.
        /// </summary>
        public PrintBorder Border { get; set; }

        /// <summary>
        /// Отступ от края элемента до содержимого элемента.
        /// </summary>
        public PrintThickness Padding { get; set; }

        /// <summary>
        /// Горизонтальное выравнивание текста элемента.
        /// </summary>
        public PrintTextAlignment? TextAlignment { get; set; }

        /// <summary>
        /// Количество ячеек для объединения по горизонтали.
        /// </summary>
        public int? ColumnSpan { get; set; }

        /// <summary>
        /// Количество ячеек для объединения по вертикали.
        /// </summary>
        public int? RowSpan { get; set; }

        /// <summary>
        /// Содержимое ячейки.
        /// </summary>
        public PrintBlock Block { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintTableCell;
        }
    }
}
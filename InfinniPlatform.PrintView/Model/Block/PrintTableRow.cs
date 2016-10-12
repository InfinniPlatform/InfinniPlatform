using System.Collections.Generic;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания строки таблицы.
    /// </summary>
    public class PrintTableRow : PrintNamedItem
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintTableRow()
        {
            Cells = new List<PrintTableCell>();
        }


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
        /// Список ячеек строки.
        /// </summary>
        public List<PrintTableCell> Cells { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintTableRow;
        }
    }
}
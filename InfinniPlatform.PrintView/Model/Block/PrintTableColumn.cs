using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания столбца таблицы.
    /// </summary>
    public class PrintTableColumn : PrintNamedItem
    {
        /// <summary>
        /// Ширина столбца.
        /// </summary>
        public double? Size { get; set; }

        /// <summary>
        /// Единица измерения ширины столбца.
        /// </summary>
        public PrintSizeUnit? SizeUnit { get; set; }

        /// <summary>
        /// Заголовок столбца.
        /// </summary>
        public PrintTableCell Header { get; set; }

        /// <summary>
        /// Шаблон ячеек столбца.
        /// </summary>
        public PrintTableCell CellTemplate { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintTableColumn;
        }
    }
}
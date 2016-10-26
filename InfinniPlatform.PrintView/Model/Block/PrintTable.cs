using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания таблицы.
    /// </summary>
    [Serializable]
    public class PrintTable : PrintBlock
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Table";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintTable()
        {
            Columns = new List<PrintTableColumn>();
            Rows = new List<PrintTableRow>();
        }


        /// <summary>
        /// Показывать ли заголовок таблицы.
        /// </summary>
        public bool? ShowHeader { get; set; }

        /// <summary>
        /// Список столбцов таблицы.
        /// </summary>
        public List<PrintTableColumn> Columns { get; set; }

        /// <summary>
        /// Список строк таблицы.
        /// </summary>
        public List<PrintTableRow> Rows { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintTable;
        }
    }
}
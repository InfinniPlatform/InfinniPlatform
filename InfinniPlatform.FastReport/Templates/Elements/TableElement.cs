using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Borders;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Таблица.
	/// </summary>
	public sealed class TableElement : IElement
	{
		/// <summary>
		/// Границы элемента.
		/// </summary>
		public Border Border { get; set; }

		/// <summary>
		/// Расположение элемента.
		/// </summary>
		public ElementLayout Layout { get; set; }


		/// <summary>
		/// Список строк таблицы.
		/// </summary>
		public ICollection<TableRow> Rows { get; set; }

		/// <summary>
		/// Список столбцов таблицы.
		/// </summary>
		public ICollection<TableColumn> Columns { get; set; }

		/// <summary>
		/// Список ячеек таблицы.
		/// </summary>
		public ICollection<TableCell> Cells { get; set; }
	}
}
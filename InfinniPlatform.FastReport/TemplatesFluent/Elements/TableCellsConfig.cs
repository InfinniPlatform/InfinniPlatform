using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки ячеек таблицы.
	/// </summary>
	public sealed class TableCellsConfig
	{
		internal TableCellsConfig(ICollection<TableCell> tableCells)
		{
			if (tableCells == null)
			{
				throw new ArgumentNullException("tableCells");
			}

			_tableCells = tableCells;
		}


		private readonly ICollection<TableCell> _tableCells;


		/// <summary>
		/// Ячейка таблицы.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public TableCellsConfig Cell(int rowIndex, int columnIndex, Action<TableCellConfig> action)
		{
			if (rowIndex < 0)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}

			if (columnIndex < 0)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}

			var tableCell = _tableCells.FirstOrDefault(i => i.RowIndex == rowIndex && i.ColumnIndex == columnIndex);

			if (tableCell == null)
			{
				tableCell = new TableCell { RowIndex = rowIndex, ColumnIndex = columnIndex };

				_tableCells.Add(tableCell);
			}

			var configuration = new TableCellConfig(tableCell);
			action(configuration);

			return this;
		}
	}
}
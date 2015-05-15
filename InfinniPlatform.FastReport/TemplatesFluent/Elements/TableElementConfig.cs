using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Borders;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки таблицы.
	/// </summary>
	public sealed class TableElementConfig
	{
		internal TableElementConfig(TableElement tableElement)
		{
			if (tableElement == null)
			{
				throw new ArgumentNullException("tableElement");
			}

			_tableElement = tableElement;
		}


		private readonly TableElement _tableElement;


		/// <summary>
		/// Границы элемента.
		/// </summary>
		public TableElementConfig Border(Action<BorderConfig> action)
		{
			if (_tableElement.Border == null)
			{
				_tableElement.Border = new Border();
			}

			var configuration = new BorderConfig(_tableElement.Border);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Расположение элемента.
		/// </summary>
		public TableElementConfig Layout(Action<ElementLayoutConfig> action)
		{
			if (_tableElement.Layout == null)
			{
				_tableElement.Layout = new ElementLayout();
			}

			var configuration = new ElementLayoutConfig(_tableElement.Layout);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Сетка таблицы.
		/// </summary>
		public TableElementConfig Grid(Action<TableRowsConfig> rows, Action<TableColumnsConfig> columns, Action<TableCellsConfig> cells)
		{
			if (_tableElement.Rows == null)
			{
				_tableElement.Rows = new List<TableRow>();
			}

			if (_tableElement.Columns == null)
			{
				_tableElement.Columns = new List<TableColumn>();
			}

			if (_tableElement.Cells == null)
			{
				_tableElement.Cells = new List<TableCell>();
			}

			var rowConfiguration = new TableRowsConfig(_tableElement.Rows);
			rows(rowConfiguration);

			var columnConfiguration = new TableColumnsConfig(_tableElement.Columns);
			columns(columnConfiguration);

			var cellConfiguration = new TableCellsConfig(_tableElement.Cells);
			cells(cellConfiguration);

			return this;
		}
	}
}
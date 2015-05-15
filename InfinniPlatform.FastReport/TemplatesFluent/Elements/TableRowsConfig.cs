using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки строк таблицы.
	/// </summary>
	public sealed class TableRowsConfig
	{
		internal TableRowsConfig(ICollection<TableRow> tableRows)
		{
			if (tableRows == null)
			{
				throw new ArgumentNullException("tableRows");
			}

			_tableRows = tableRows;
		}


		private readonly ICollection<TableRow> _tableRows;


		/// <summary>
		/// Строка таблицы.
		/// </summary>
		public TableRowsConfig Row(Action<TableRowConfig> action)
		{
			var tableRow = new TableRow { Index = _tableRows.Count };

			_tableRows.Add(tableRow);

			var configuration = new TableRowConfig(tableRow);
			action(configuration);

			return this;
		}
	}
}
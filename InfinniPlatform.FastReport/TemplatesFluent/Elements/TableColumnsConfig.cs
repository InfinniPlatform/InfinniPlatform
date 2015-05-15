using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки колонок таблицы.
	/// </summary>
	public sealed class TableColumnsConfig
	{
		internal TableColumnsConfig(ICollection<TableColumn> tableColumns)
		{
			if (tableColumns == null)
			{
				throw new ArgumentNullException("tableColumns");
			}

			_tableColumns = tableColumns;
		}


		private readonly ICollection<TableColumn> _tableColumns;


		/// <summary>
		/// Колонка таблицы.
		/// </summary>
		public TableColumnsConfig Column(Action<TableColumnConfig> action)
		{
			var tableColumn = new TableColumn { Index = _tableColumns.Count };

			_tableColumns.Add(tableColumn);

			var configuration = new TableColumnConfig(tableColumn);
			action(configuration);

			return this;
		}
	}
}
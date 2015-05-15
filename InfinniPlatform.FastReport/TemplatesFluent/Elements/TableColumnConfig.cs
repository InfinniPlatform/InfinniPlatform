using System;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки столбца таблицы.
	/// </summary>
	public sealed class TableColumnConfig
	{
		internal TableColumnConfig(TableColumn tableColumn)
		{
			if (tableColumn == null)
			{
				throw new ArgumentNullException("tableColumn");
			}

			_tableColumn = tableColumn;
		}


		private readonly TableColumn _tableColumn;


		/// <summary>
		/// Ширина.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public TableColumnConfig Width(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_tableColumn.Width = value;

			return this;
		}

		/// <summary>
		/// Ширина определяется автоматически.
		/// </summary>
		public TableColumnConfig AutoWidth()
		{
			_tableColumn.AutoWidth = true;

			return this;
		}
	}
}
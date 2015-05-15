using System;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки строки таблицы.
	/// </summary>
	public sealed class TableRowConfig
	{
		internal TableRowConfig(TableRow tableRow)
		{
			if (tableRow == null)
			{
				throw new ArgumentNullException("tableRow");
			}

			_tableRow = tableRow;
		}


		private readonly TableRow _tableRow;


		/// <summary>
		/// Высота.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public TableRowConfig Height(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_tableRow.Height = value;

			return this;
		}

		/// <summary>
		/// Высота определяется автоматически.
		/// </summary>
		public TableRowConfig AutoHeight()
		{
			_tableRow.AutoHeight = true;

			return this;
		}
	}
}
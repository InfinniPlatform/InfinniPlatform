using System;

using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Borders;
using InfinniPlatform.FastReport.TemplatesFluent.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Formats;

namespace InfinniPlatform.FastReport.TemplatesFluent.Elements
{
	/// <summary>
	/// Интерфейс для настройки ячейки таблицы.
	/// </summary>
	public sealed class TableCellConfig
	{
		internal TableCellConfig(TableCell tableCell)
		{
			if (tableCell == null)
			{
				throw new ArgumentNullException("tableCell");
			}

			_tableCell = tableCell;
		}


		private readonly TableCell _tableCell;


		/// <summary>
		/// Источник данных.
		/// </summary>
		public TableCellConfig Bind(Action<DataBindConfig> action)
		{
			var configuration = new DataBindConfig(dataBind => _tableCell.DataBind = dataBind);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Начертание текста.
		/// </summary>
		public TableCellConfig Style(Action<TextElementStyleConfig> action)
		{
			if (_tableCell.Style == null)
			{
				_tableCell.Style = new TextElementStyle();
			}

			var configuration = new TextElementStyleConfig(_tableCell.Style);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Формат отображения.
		/// </summary>
		public TableCellConfig Format(Action<FormatConfig> action)
		{
			var configuration = new FormatConfig(_tableCell);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Границы элемента.
		/// </summary>
		public TableCellConfig Border(Action<BorderConfig> action)
		{
			if (_tableCell.Border == null)
			{
				_tableCell.Border = new Border();
			}

			var configuration = new BorderConfig(_tableCell.Border);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Объединить ячейку с соседними справа.
		/// </summary>
		public TableCellConfig ColSpan(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_tableCell.ColSpan = value;

			return this;
		}

		/// <summary>
		/// Объединить ячейку с соседними снизу.
		/// </summary>
		public TableCellConfig RowSpan(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_tableCell.RowSpan = value;

			return this;
		}
	}
}